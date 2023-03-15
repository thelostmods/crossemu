using System.IO;
using System.Text.Json.Serialization;

using Asfw.IO;

public class SettingsDef
{
	// general
	[JsonPropertyName("Settings Version")]
	public float Version;
	const float VERSION = 0.5f;

	[JsonPropertyName("Selected Console")]
	public string ActiveConsole { get; set; } = "Nil";


		// backup
	[JsonPropertyName("Automatic Backup")]
	public BackupDef Backup { get; set; } = new();
	public class BackupDef
	{
		public bool Enabled { get; set; } = true;

		[JsonPropertyName("Time (In seconds)")]
		public int Time { get; set; } = 30;
	}
		

	// window
	[JsonPropertyName("Window Bounds")]
	public Godot.Rect2I Window { get; set; } = new(200, 100, 800, 400);

		
	// console settings


	// // n64
	[JsonPropertyName("[N64] Settings")]
	public N64Def N64 { get; set; } = new();
	public class N64Def
	{
		[JsonPropertyName("Plugin-Audio")]
		public string PlugAudio { get; set; } = "mupen64plus-audio-sdl";

		[JsonPropertyName("Plugin-Input")]
		public string PlugInput { get; set; } = "mupen64plus-input-sdl";

		[JsonPropertyName("Plugin-Rsp")]
		public string PlugRsp { get; set; } = "mupen64plus-rsp-hle";

		[JsonPropertyName("Plugin-Video")]
		public string PlugVideo { get; set; } = "mupen64plus-video-gliden64";
	}

	public static void LoadFile()
	{
		string path = Paths.SettingsFile();

		// make file if one does not exist
		if (!File.Exists(path))
		{
			File.Create(path).Dispose();
			Serialization.SaveJson(path, new SettingsDef());
		}

		// load file data from json
		Program.Settings = Serialization.LoadJson<SettingsDef>(path);

		// ensure current version format
		if (Program.Settings.Version != SettingsDef.VERSION)
			Program.Settings.Version = SettingsDef.VERSION;
			Serialization.SaveJson(path, Program.Settings);
	}

	public static void SaveFile()
	{
		string path = Paths.SettingsFile();
		Serialization.SaveJson(path, Program.Settings);
	}
}
