internal class Console
{
    public virtual ID GetID() => ID.Nil;
    public enum ID
    {
        Nil,
        // NES,
        // SNES,
        // GBC,
        // GBA,
        N64,
        // GC,
        // Wii,
        // WiiU,
        // Switch,
    }

    public virtual string[] AvailableExtensions { get; } = new string[0];
    public virtual bool Running { get; set; } = false;

    // install/setup
    protected virtual bool CheckEmulator() => true;
    protected virtual bool CheckRuntimes() => true;
    public bool VerifyInstall(bool printError = false)
    {
        // verify the emulator exists
        if (!CheckEmulator())
        {
            if (printError)
                Godot.OS.Alert("Requested emulator is missing!", "Missing Emulator");
            return false;
        }
            
        // verify the runtimes for emulator to run are installed
        if (!CheckRuntimes())
        {
            if (printError)
                Godot.OS.Alert("Emulator runtimes are not installed!", "Missing Runtimes");
            return false;
        }
            
        // everything checks out
        return true;
    }

    // runtime
    public virtual void Boot(string gamePath) {}
}