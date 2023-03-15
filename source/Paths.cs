using System;

internal static class Paths
{
	///////////////////////////////////////
	/// Utility
	//////////////////////////////

	private static string Ensure(string path)
	{
		if (!System.IO.Directory.Exists(path))
			System.IO.Directory.CreateDirectory(path);
		return path;
	}

	///////////////////////////////////////
	/// Routing Paths
	//////////////////////////////

	/// <summary>Returns the Cross-Emu app binary directory.</summary>
	public static string App() => AppContext.BaseDirectory;

	/// <summary>(Probably) Used for godot-editor rom-hacking (RomEdit) mode.</summary>
	public static string GetDirectory(bool external) =>
		external ? Godot.OS.GetUserDataDir() + "/" : App();

	///////////////////////////////////////
	/// Cross-Emu User Paths
	//////////////////////////////

	/// <summary>Returns the 'consoles' folder.</summary>
	public static string ConsoleRoot() =>
		Ensure(Godot.OS.GetUserDataDir() + "/consoles/");

	/// <summary>Returns the specified consoles content folder.</summary>
	public static string Consoles(Console.ID id) =>
		Ensure(ConsoleRoot() + id.ToString().ToLower() + "/");

	/// <summary>Returns the specified consoles emulator files folder.</summary>
	public static string Emulator(Console.ID id) =>
		Ensure(Consoles(id) + "emulator/");

	/// <summary>Returns the specified consoles rom files folder.</summary>
	public static string Games(Console.ID id) =>
		Ensure(Consoles(id) + "games/");

	/// <summary>Returns the specified consoles mods folder.</summary>
	public static string Mods(Console.ID id) =>
		Ensure(Consoles(id) + "mods/");

	/// <summary>Returns the specified mod-settings file.</summary>
	public static string ModSettings(Console.ID id, string modName) =>
		Consoles(id) + "settings/" + modName + ".json";

	/// <summary>Returns the Cross-Emu plugins (extensions) folder.</summary>
	public static string Plugins() =>
		Ensure(Godot.OS.GetUserDataDir() + "/plugins/");

	/// <summary>Returns the settings file path.</summary>
	public static string SettingsFile() =>
		Godot.OS.GetUserDataDir() + "/settings.json";
}