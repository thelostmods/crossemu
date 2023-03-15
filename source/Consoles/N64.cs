using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using Godot;

using CrossEmu.Sdk;
using CrossEmu.Sdk.N64;

using Mupen64Plus_DotNet;

internal class ConsoleN64 : Console
{
    public override ID GetID() => ID.N64;

    public override string[] AvailableExtensions { get; } = {
        "*.z64 ; N64 Rom (BE)",
        "*.v64 ; N64 Rom (RE)",
        "*.n64 ; N64 Rom (LE)"
    };

    // install/setup
    protected override bool CheckEmulator() => true;
    protected override bool CheckRuntimes() => true;

    // runtime
    public override void Boot(string gamePath)
    {
       	m64p_error err;

        // load emulator
        err = LoadEmulator();
        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Failed to load the mupen core.");
            return;
        }

        // try to load rom into emulator
        err = LoadGame(gamePath);
        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Failed to open the ROM image file.");
            UnloadEmulator();
            return;
        }

        // load plugins
        err = M64P.AttachPlugins(
            DebugCallback,
            Program.Settings.N64.PlugVideo,
            Program.Settings.N64.PlugAudio,
            Program.Settings.N64.PlugInput,
            Program.Settings.N64.PlugRsp,
            Paths.Emulator(Console.ID.N64)
        );
        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Failed to attach plugins, exiting process.");
            UnloadEmulator();
            return;
        }

        // attach startup callback
        var pInit = Marshal.GetFunctionPointerForDelegate(InitN64Sdk);
        err = M64P.CoreDoCommand!(m64p_command.SET_FRAME_CALLBACK, 0, pInit);
        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Failed to create communication layer, exiting process.");
            UnloadEmulator();
            return;
        }

    	// load all mods
    	ModLoader.Recache(Paths.Mods(Console.ID.N64));
    	ModLoader.TmpLoadAll();

        // attempt execute runtime
        Program.EmulatorThread = new Thread(new ThreadStart(() =>
            {
                // mark ourselves as running
                Program.ActiveConsole.Running = true;

                // run the emulator
                var err = M64P.CoreDoCommand!(m64p_command.EXECUTE, 0, IntPtr.Zero);
                if (err != m64p_error.SUCCESS)
                {
                    GD.Print("CrossEmulator: N64 closed unexpectedly!");
                    GD.Print(err.ToString());
                }

                // game was finished -- clean up
                ModLoader.UnloadAll();
                UnloadEmulator();

                // kill thread
                Program.EmulatorThread!.Join();
                Program.EmulatorThread = null;
            }
        )); Program.EmulatorThread.Start();
    }


    // local static handlers


    private static m64p_error LoadEmulator()
    {
        // check if already attached
        if (M64P.CoreIsLoaded) return m64p_error.SUCCESS;

        // load the lib
        M64P.LoadCore(Paths.Emulator(Console.ID.N64));
        if (!M64P.CoreIsLoaded)
        {
            GD.Print("CrossEmulator: Failed to find Mupen64Plus Core library");
            return m64p_error.INPUT_NOT_FOUND;
        }

        // initialize the core lib
        var libName = Marshal.StringToHGlobalAnsi("mupen64plus-core");
        var libDir = Paths.Emulator(Console.ID.N64);
        var libVer = 0x020001;
        var err = M64P.CoreStartup!(libVer, libDir, libDir, libName,
                        DebugCallback, IntPtr.Zero, null!);
            
        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Couldn't start Mupen64Plus core library.");
            UnloadEmulator();
        }

        return err;
    }

    private static m64p_error UnloadEmulator()
    {
        var err = M64P.UnloadCore();

        // mark ourselves no longer running
        Program.ActiveConsole.Running = false;
        
        return err;
    }

    private static m64p_error LoadGame(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        IntPtr ptr = handle.AddrOfPinnedObject();

        var err = M64P.CoreDoCommand!(m64p_command.ROM_OPEN, data.Length, (IntPtr) ptr);

        handle.Free();

        if (err != m64p_error.SUCCESS)
        {
            GD.Print("CrossEmulator: Core failed to open ROM image file.");
            UnloadEmulator();
        }

        return err;
    }

    private static void InitN64Sdk()
    {

        // prepare sdk data for mods
        N64RomHeader.Ptr = M64P.GetHeader!();

        N64RdRam.Ptr = M64P.GetRdRam!();
        N64RdRam.Len = M64P.GetRdRamSize!();

        N64Rom.Ptr = M64P.GetRom!();
        N64Rom.Len = M64P.GetRomSize!();
            
        // update discord rpc
        Discord.RpcSetGame("n64", Utility.ToHex(N64RomHeader.GetGameID()), N64RomHeader.GetName());

        // pass the first frame call to the plugins
        ModLoader.OnFirstFrame();

        // hookup the tick to finalize
        var pOnTick = Marshal.GetFunctionPointerForDelegate(ModLoader.OnTickCallback);
        if (M64P.CoreDoCommand!(m64p_command.SET_FRAME_CALLBACK, 0, pOnTick) != m64p_error.SUCCESS) return;
    }


    // Placeholder until working terminal
    // TODO: Fix this.
    static bool IsVerbose = false;
    static M64P.DebugCallback DebugCallback = LogMupen;
    static void LogMupen(IntPtr context, int level, string message)
    {
        var lib = Marshal.PtrToStringAnsi(context);

        switch (level)
        {
            case (int) m64p_msg_level.ERROR:   GD.Print($"{lib} Error: {message}");   break;
            case (int) m64p_msg_level.WARNING: GD.Print($"{lib} Warning: {message}"); break;
            case (int) m64p_msg_level.INFO:    GD.Print($"{lib} Info: {message}");    break;
            case (int) m64p_msg_level.STATUS:  GD.Print($"{lib} Status: {message}");  break;

            case (int) m64p_msg_level.VERBOSE:
                if (IsVerbose) GD.Print($"{lib}: {message}");
                break;

            default:
                GD.Print($"{lib} Unknown: {message}");
                break;
        }
    }
}