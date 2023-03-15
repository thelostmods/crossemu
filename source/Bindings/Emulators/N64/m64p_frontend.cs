using System;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		/* pointer types to the callback functions in the front-end application */
		public delegate void DebugCallback(IntPtr Context, int level, string message);
		public delegate void StateCallback(IntPtr Context, m64p_core_param param_type, int new_value);

		/// <summary>
		/// CoreStartup()
		///
		/// This function initializes libmupen64plus for use by allocating memory,
		/// creating data structures, and loading the configuration file.
		/// </summary>
		public static d_CoreStartup? CoreStartup;
		public delegate m64p_error d_CoreStartup(int var1, string var2, string var3, IntPtr var4, DebugCallback var5, IntPtr var6, StateCallback var7);

		/// <summary>
		/// CoreShutdown()
		///
		/// This function saves the configuration file, then destroys data structures
		/// and releases memory allocated by the core library.
		/// </summary>
		public static d_CoreShutdown? CoreShutdown;
		public delegate m64p_error d_CoreShutdown();

		/// <summary>
		/// CoreAttachPlugin()
		///
		/// This function attaches the given plugin to the emulator core.There can only
		/// be one plugin of each type attached to the core at any given time. 
		/// </summary>
		public static d_CoreAttachPlugin? CoreAttachPlugin;
		public delegate m64p_error d_CoreAttachPlugin(m64p_plugin_type var1, IntPtr var2);

		/// <summary>
		/// CoreDetachPlugin()
		///
		/// This function detaches the given plugin from the emulator core, and re-attaches
		/// the 'dummy' plugin functions.
		/// </summary>
		public static d_CoreDetachPlugin? CoreDetachPlugin;
		public delegate m64p_error d_CoreDetachPlugin(m64p_plugin_type var1);

		/// <summary>
		/// CoreDoCommand()
		///
		/// This function sends a command to the emulator core.
		/// </summary>
		public static d_CoreDoCommand? CoreDoCommand;
		public delegate m64p_error d_CoreDoCommand(m64p_command var1, int var2, IntPtr var3);

		/// <summary>
		/// CoreOverrideVidExt()
		///
		/// This function overrides the core's internal SDL-based OpenGL functions. This
		/// override functionality allows a front-end to define its own video extension
		/// functions to be used instead of the SDL functions.If any of the function
		/// pointers in the structure are NULL, the override function will be disabled
		/// and the core's internal SDL functions will be used.
		/// </summary>
		public static d_CoreOverrideVidExt? CoreOverrideVidExt;
		public delegate m64p_error d_CoreOverrideVidExt(ref m64p_video_extension_functions var1);

		/// <summary>
		/// CoreAddCheat()
		///
		/// This function will add a Cheat Function to a list of currently active cheats
		/// which are applied to the open ROM.
		/// </summary>
		public static d_CoreAddCheat? CoreAddCheat;
		public delegate m64p_error d_CoreAddCheat(string var1, ref m64p_cheat_code var2, int var3);

		/// <summary>
		/// CoreCheatEnabled()
		///
		/// This function will enable or disable a Cheat Function which is in the list of
		/// currently active cheats.
		/// </summary>
		public static d_CoreCheatEnabled? CoreCheatEnabled;
		public delegate m64p_error d_CoreCheatEnabled(ref m64p_rom_settings var1, int var2, int var3, int var4);

		/// <summary>
		/// CoreGetRomSettings()
		///
		/// This function will retrieve the ROM settings from the mupen64plus INI file for
		/// the ROM image corresponding to the given CRC values.
		/// </summary>
		public static d_CoreGetRomSettings? CoreGetRomSettings;
		public delegate m64p_error d_CoreGetRomSettings(ref m64p_rom_settings var1, int var2, int var3, int var4);

		/// <summary>
		/// CoreSaveOverride()
		///
		/// This function will override where the eep save files are targetting.
		/// </summary>
		public static d_CoreSaveOverride? CoreSaveOverride;
		public delegate m64p_error d_CoreSaveOverride(string path);

		/***********************\
		|*  MODDING FUNCTIONS  *|
		\***********************/

		/// <summary>
		/// GetHeader()
		///
		/// This function will return a pointer to the ROM_HEADER.
		/// </summary>
		public static d_GetHeader? GetHeader;
		public delegate IntPtr d_GetHeader();

		/// <summary>
		/// GetRdRam()
		///
		/// This function will return a pointer to the RDRAM.
		/// </summary>
		public static d_GetRdRam? GetRdRam;
		public delegate IntPtr d_GetRdRam();

		/// <summary>
		/// GetRom()
		///
		/// This function will return a pointer to the ROM.
		/// </summary>
		public static d_GetRom? GetRom;
		public delegate IntPtr d_GetRom();

		/// <summary>
		/// GetRdRamSize()
		///
		/// This function will return the real length of RDRAM memory.
		/// </summary>
		public static d_GetRdRamSize? GetRdRamSize;
		public delegate UIntPtr d_GetRdRamSize();

		/// <summary>
		/// GetRomSize()
		///
		/// This function will return the real length of ROM memory.
		/// </summary>
		public static d_GetRomSize? GetRomSize;
		public delegate UIntPtr d_GetRomSize();

		/// <summary>
		/// RefreshDynarec()
		///
		/// This function will refresh the dynamic recompiler needed for asm injections to work.
		/// </summary>
		public static d_RefreshDynarec? RefreshDynarec;
		public delegate void d_RefreshDynarec();
	}
}
