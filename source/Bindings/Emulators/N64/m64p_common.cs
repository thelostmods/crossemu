using System;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		/// <summary>
		/// PluginGetVersion()
		/// 
		/// This function retrieves version information from a library. This
		/// function is the same for the core library and the plugins.
		/// </summary>
		public static d_PluginGetVersion? PluginGetVersion;
		public delegate m64p_error d_PluginGetVersion(ref m64p_plugin_type var1, ref int var2, ref int var3, ref string var4, ref int var5);

		/// <summary>
		/// CoreGetAPIVersions()
		///
		/// This function retrieves API version information from the core.
		/// </summary>
		public static d_CoreGetAPIVersions? CoreGetAPIVersions;
		public delegate m64p_error d_CoreGetAPIVersions(ref int var1, ref int var2, ref int var3, ref int var4);

		/// <summary>
		/// CoreErrorMessage()
		///
		/// This function returns a pointer to a NULL-terminated string giving a
		/// human-readable description of the error.
		/// </summary>
		public static d_CoreErrorMessage? CoreErrorMessage;
		public delegate string d_CoreErrorMessage(m64p_error var1);

		/// <summary>
		/// PluginStartup()
		///
		/// This function initializes a plugin for use by allocating memory, creating
		/// data structures, and loading the configuration data.
		/// </summary>
		public static d_PluginStartup? PluginStartup;
		public delegate m64p_error d_PluginStartup(IntPtr var1, IntPtr var2, DebugCallback var3);

		/// <summary>
		/// PluginShutdown()
		///
		/// This function destroys data structures and releases memory allocated by
		/// the plugin library.
		/// </summary>
		public static d_PluginShutdown? PluginShutdown;
		public delegate m64p_error d_PluginShutdown();
	}
}
