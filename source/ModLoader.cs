using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using DynaLib_DotNet;

internal static class ModLoader
{
	struct PluginDef
	{
		public DynaLib lib;

		// natives
		/* */ public delegate IntPtr ptr_ret_func();
		/* */ public delegate uint uint_ret_func();
		/* */ public delegate bool bool_ret_func();
		/* */ public delegate void void_ret_func();
		/* */ public delegate void frame_param_func(uint curFrame);

		public ptr_ret_func plugin_name;
		public ptr_ret_func plugin_description;
		public ptr_ret_func plugin_authors;
		public uint_ret_func plugin_version;
		public bool_ret_func plugin_req_boot;

		public bool_ret_func plugin_initialize;
		public void_ret_func plugin_terminate;
		public void_ret_func plugin_on_first_frame;
		public frame_param_func plugin_on_tick;
	
		// properties
		public bool Running;
		public string GetName() => plugin_name != null ? Marshal.PtrToStringAuto(plugin_name())! : "N/A";
		public string GetDescription() => plugin_description != null ? Marshal.PtrToStringAuto(plugin_description())! : "N/A";
		public string GetAuthors() => plugin_authors != null ? Marshal.PtrToStringAuto(plugin_authors())! : "N/A";
		public uint GetVersion() => plugin_version != null ? plugin_version() : 0x00;
		public bool GetReqBoot() => plugin_req_boot != null ? plugin_req_boot() : false;

		public string GetVersionString()
		{
			uint v = GetVersion();
			return $"{(v >> 24) & 0xFF}.{(v >> 16) & 0xFF}.{(v >> 8) & 0xFF}.{v & 0xFF}";
		}
	}

	static readonly Dictionary<string, string> cache = new();
	static readonly Dictionary<string, PluginDef> mods = new();

	public static void Recache(string path)
	{
		// reset cache files
		cache.Clear();

		// prepare search type
		string? ext;
		if (OperatingSystem.IsLinux()) ext = "*.so";
		else if (OperatingSystem.IsMacOS()) ext = "*.dylib";
		else if (OperatingSystem.IsWindows()) ext = "*.dll";
		else throw new Exception("System not compatible with mods!");

		// grab libs reguardless of sub-directories
		var di = new DirectoryInfo(path);
		foreach (var entry in di.GetFiles(ext, SearchOption.AllDirectories))
			cache[entry.Name[..^entry.Extension.Length]] = entry.FullName;
	}

	public static void TmpLoadAll()
	{
		foreach (var mod in cache.Keys)
			Load(mod);
	}

	public static void Load(string modName, bool withBoot = false)
	{
		/* tried to load an invalid file */
		if (!cache.ContainsKey(modName)) return;

		var mod = new PluginDef();
		if (!TryLoad(modName, ref mod))
		{
			Godot.GD.Print("There was a problem loading mod from file '" + modName + "'!");
			cache.Remove(modName);
			return;
		}

		if (!mod.lib.IsLoaded())
		{
			Godot.GD.Print("There was a severe problem! Possible memory leak.");
			cache.Remove(modName);
			return;
		}

		if (!mod.GetReqBoot() || withBoot)
		{
			mod.Running = true;
			if (!mod.plugin_initialize())
			{
				mod.plugin_terminate();
				cache.Remove(modName);
				return;
			}
		}

		mods[modName] = mod;
	}

	static bool TryLoad(string modName, ref PluginDef mod)
	{
		var file = cache[modName];
		var fi = new FileInfo(file);

		if (!File.Exists(file) || fi == null)
		{
			Godot.GD.Print("File at '" + file + "' could not be found.");
			cache.Remove(modName);
			return false;
		}

		mod.lib.Load(modName, fi.DirectoryName!, "!");

		if (!mod.lib.IsLoaded())
			throw new Exception("Tried to load an invalid file at path '" + file + "'.");

		if (
			!mod.lib.Bind(ref mod.plugin_name, "plugin_name") ||
			!mod.lib.Bind(ref mod.plugin_description, "plugin_description") ||
			!mod.lib.Bind(ref mod.plugin_authors, "plugin_authors") ||
			!mod.lib.Bind(ref mod.plugin_version, "plugin_version") ||
			!mod.lib.Bind(ref mod.plugin_req_boot, "plugin_req_restart") ||

			!mod.lib.Bind(ref mod.plugin_initialize, "plugin_initialize") ||
			!mod.lib.Bind(ref mod.plugin_terminate, "plugin_terminate") ||
			!mod.lib.Bind(ref mod.plugin_on_first_frame, "plugin_on_first_frame") ||
			!mod.lib.Bind(ref mod.plugin_on_tick, "plugin_on_tick")
		)
		{
			Godot.GD.Print("CrossEmulator: File '" + file + "' was missing 1 or more required definitions!");
			Godot.GD.Print("               Unloading the plugin.");
			mod.lib.Unload();
			return false;
		}

		return true;
	}

	static void Unload(PluginDef mod)
	{
		mod.plugin_terminate();
		mod.lib.Unload();
	}

	public static void UnloadAll()
	{
		foreach (var m in mods.Values)
			Unload(m);

		mods.Clear();
		Program.CollectGarbage();
	}

	public static void OnFirstFrame()
	{
		foreach (var m in mods.Values)
			if (m.Running) m.plugin_on_first_frame();
	}

	public delegate void FrameCallback(uint curFrame);
	public static FrameCallback OnTickCallback = OnTick;
	public static void OnTick(uint curFrame)
	{
		foreach (var m in mods.Values)
			if (m.Running) m.plugin_on_tick(curFrame);
	}
}
