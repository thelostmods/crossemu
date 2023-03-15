using System;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		/// <summary>
		/// ConfigListSections()
		///
		/// This function is called to enumerate the list of Sections in the Mupen64Plus
		/// configuration file.It is expected that there will be a section named "Core"
		/// for core-specific configuration data, "Graphics" for common graphics options,
		/// and one or more sections for each plugin library.
		/// </summary>
		public static d_ConfigListSections? ConfigListSections;
		public delegate m64p_error d_ConfigListSections(IntPtr var1, d2_ConfigListSections? var2);
		public delegate void d2_ConfigListSections(IntPtr var1, string var2);

		/// <summary>
		/// ConfigOpenSection()
		///
		/// This function is used to give a configuration section handle to the front-end
		/// which may be used to read or write configuration parameter values in a given
		/// section of the configuration file.
		/// </summary>
		public static d_ConfigOpenSection? ConfigOpenSection;
		public delegate m64p_error d_ConfigOpenSection(string var1, ref IntPtr var2);

		/// <summary>
		/// ConfigListParameters()
		///
		/// This function is called to enumerate the list of Parameters in a given
		/// Section of the Mupen64Plus configuration file.
		/// </summary>
		public static d_ConfigListParameters? ConfigListParameters;
		public delegate m64p_error d_ConfigListParameters(IntPtr var1, IntPtr var2, d3_ConfigListParameters? var3);
		public delegate void d3_ConfigListParameters(IntPtr var1, string var2, m64p_type var3);

		/// <summary>
		/// ConfigSaveFile()
		///
		/// This function saves the entire current Mupen64Plus configuration to disk.
		/// </summary>
		public static d_ConfigSaveFile? ConfigSaveFile;
		public delegate m64p_error d_ConfigSaveFile();

		/// <summary>
		/// ConfigSaveSection()
		///
		/// This function saves one section of the current Mupen64Plus configuration to disk.
		/// </summary>
		public static d_ConfigSaveSection? ConfigSaveSection;
		public delegate m64p_error d_ConfigSaveSection(string var1);

		/// <summary>
		/// ConfigHasUnsavedChanges()
		///
		/// This function determines if a given Section(or all sections) of the 
		// Mupen64Plus Core configuration file has been modified since it was 
		// last saved or loaded.
		/// </summary>
		public static d_ConfigHasUnsavedChanges? ConfigHasUnsavedChanges;
		public delegate int d_ConfigHasUnsavedChanges(string var1);

		/// <summary>
		/// ConfigDeleteSection()
		///
		/// This function deletes a section from the Mupen64Plus configuration data.
		/// </summary>
		public static d_ConfigDeleteSection? ConfigDeleteSection;
		public delegate m64p_error d_ConfigDeleteSection(string SectionName);

		/// <summary>
		/// ConfigRevertChanges()
		///
		/// This function reverts changes previously made to one section of the 
		/// configuration file, so that it will match with the configuration at 
		// the last time that it was loaded from or saved to disk.
		/// </summary>
		public static d_ConfigRevertChanges? ConfigRevertChanges;
		public delegate m64p_error d_ConfigRevertChanges(string SectionName);

		/// <summary>
		/// ConfigSetParameter()
		///
		/// This function sets the value of one of the emulator's configuration
		/// parameters.
		/// </summary>
		public static d_ConfigSetParameter? ConfigSetParameter;
		public delegate m64p_error d_ConfigSetParameter(IntPtr var1, string var2, m64p_type var3, IntPtr var4);

		/// <summary>
		/// ConfigSetParameterHelp()
		///
		/// This function sets the help string of one of the emulator's configuration
		/// parameters.
		/// </summary>
		public static d_ConfigSetParameterHelp? ConfigSetParameterHelp;
		public delegate m64p_error d_ConfigSetParameterHelp(IntPtr var1, string var2, string var3);

		/// <summary>
		/// ConfigGetParameter()
		///
		/// This function retrieves the value of one of the emulator's parameters. 
		/// </summary>
		public static d_ConfigGetParameter? ConfigGetParameter;
		public delegate m64p_error d_ConfigGetParameter(IntPtr var1, string var2, m64p_type var3, ref IntPtr var4, int var5);

		/// <summary>
		/// ConfigGetParameterType()
		///
		/// This function retrieves the type of one of the emulator's parameters. 
		/// </summary>
		public static d_ConfigGetParameterType? ConfigGetParameterType;
		public delegate m64p_error d_ConfigGetParameterType(IntPtr var1, string var2, ref m64p_type var3);

		/// <summary>
		/// ConfigGetParameterHelp()
		///
		/// This function retrieves the help information about one of the emulator's
		/// parameters.
		/// </summary>
		public static d_ConfigGetParameterHelp? ConfigGetParameterHelp;
		public delegate string d_ConfigGetParameterHelp(IntPtr var1, string var2);

		///
		/// ConfigSetDefault***()
		///
		/// These functions are used to set the value of a configuration parameter if it
		/// is not already present in the configuration file.This may happen if a new
		///
		/// user runs the emulator, or an upgraded module uses a new parameter, or the
		/// user deletes his or her configuration file.If the parameter is already
		/// present in the given section of the configuration file, then no action will
		/// be taken and this function will return successfully.
		///
		public static d_ConfigSetDefaultInt? ConfigSetDefaultInt;
		public static d_ConfigSetDefaultFloat? ConfigSetDefaultFloat;
		public static d_ConfigSetDefaultBool? ConfigSetDefaultBool;
		public static d_ConfigSetDefaultString? ConfigSetDefaultString;
		
		public delegate m64p_error d_ConfigSetDefaultInt(IntPtr var1, string var2, int var3, string var4);
		public delegate m64p_error d_ConfigSetDefaultFloat(IntPtr var1, string var2, float var3, string var4);
		public delegate m64p_error d_ConfigSetDefaultBool(IntPtr var1, string var2, int var3, string var4);
		public delegate m64p_error d_ConfigSetDefaultString(IntPtr var1, string var2, string var3, string var4);

		///
		/// ConfigGetParam***()
		///
		/// These functions retrieve the value of one of the emulator's parameters in
		/// the given section, and return the value directly to the calling function.If
		/// an errors occurs(such as an invalid Section handle, or invalid
		/// configuration parameter name), then an error will be sent to the front-end
		/// via the DebugCallback() function, and either a 0 (zero) or an empty string
		/// will be returned.
		///
		public static d_ConfigGetParamInt? ConfigGetParamInt;
		public static d_ConfigGetParamFloat? ConfigGetParamFloat;
		public static d_ConfigGetParamBool? ConfigGetParamBool;
		public static d_ConfigGetParamString? ConfigGetParamString;
		
		public delegate int d_ConfigGetParamInt(IntPtr var1, string var2);
		public delegate float d_ConfigGetParamFloat(IntPtr var1, string var2);
		public delegate int d_ConfigGetParamBool(IntPtr var1, string var2);
		public delegate string d_ConfigGetParamString(IntPtr var1, string var2);

		/// <summary>
		/// ConfigGetSharedDataFilepath()
		///
		/// This function is provided to allow a plugin to retrieve a full pathname to a
		/// given shared data file.This type of file is intended to be shared among
		/// multiple users on a system, so it is likely to be read-only.
		/// </summary>
		public static d_ConfigGetSharedDataFilepath? ConfigGetSharedDataFilepath;
		public delegate string d_ConfigGetSharedDataFilepath(string var1);

		/// <summary>
		/// ConfigGetUserConfigPath()
		///
		/// This function may be used by the plugins or front-end to get a path to the
		/// directory for storing user-specific configuration files.This will be the
		/// directory where "mupen64plus.cfg" is located.
		/// </summary>
		public static d_ConfigGetUserConfigPath? ConfigGetUserConfigPath;
		public delegate string d_ConfigGetUserConfigPath();

		/// <summary>
		/// ConfigGetUserDataPath()
		///
		/// This function may be used by the plugins or front-end to get a path to the
		/// directory for storing user-specific data files.This may be used to store
		/// files such as screenshots, saved game states, or hi-res textures.
		/// </summary>
		public static d_ConfigGetUserDataPath? ConfigGetUserDataPath;
		public delegate string d_ConfigGetUserDataPath();

		/// <summary>
		/// ConfigGetUserCachePath()
		///
		/// This function may be used by the plugins or front-end to get a path to the
		/// directory for storing cached user-specific data files.Files in this
		/// directory may be deleted by the user to save space, so critical information
		/// should not be stored here.This directory may be used to store files such
		/// as the ROM browser cache.
		/// </summary>
		public static d_ConfigGetUserCachePath? ConfigGetUserCachePath;
		public delegate string d_ConfigGetUserCachePath();

		/// <summary>
		/// ConfigExternalOpen()
		///
		/// This function reads the contents of the config file into memory
		/// and returns M64ERR_SUCCESS if successful.
		/// </summary>
		public static d_ConfigExternalOpen? ConfigExternalOpen;
		public delegate m64p_error d_ConfigExternalOpen(string var1, ref IntPtr var2);

		/// <summary>
		/// ConfigExternalClose()
		///
		/// Frees the memory pointer created by ConfigExternalOpen.
		/// </summary>
		public static d_ConfigExternalClose? ConfigExternalClose;
		public delegate m64p_error d_ConfigExternalClose(IntPtr var1);

		/// <summary>
		/// ConfigExternalGetParameter()
		///
		/// This functions allows a plugin to leverage the built-in ini parser to read
		/// any cfg/ini file.It will return M64ERR_SUCCESS if the item was found.
		/// </summary>
		public static d_ConfigExternalGetParameter? ConfigExternalGetParameter;
		public delegate m64p_error d_ConfigExternalGetParameter(IntPtr var1, string var2, string var3, ref string var4, int var5);

		/// <summary>
		/// ConfigSendNetplayConfig()
		///
		/// This function allows plugins to take advantage of the netplay TCP connection
		/// to send configuration data to the netplay server.
		/// </summary>
		public static d_ConfigSendNetplayConfig? ConfigSendNetplayConfig;
		public delegate m64p_error d_ConfigSendNetplayConfig(ref string var1, int var2);

		/// <summary>
		/// ConfigReceiveNetplayConfig()
		///
		/// This function allows plugins to take advantage of the netplay TCP connection
		/// to receive configuration data from the netplay server.
		/// </summary>
		public static d_ConfigReceiveNetplayConfig? ConfigReceiveNetplayConfig;
		public delegate m64p_error d_ConfigReceiveNetplayConfig(ref string var1, int var2);

		/// <summary>
		/// ConfigOverrideUserPaths()
		///
		/// This function allows overriding the paths returned by
		/// ConfigGetUserDataPath and ConfigGetUserCachePath
		/// </summary>
		public static d_ConfigOverrideUserPaths? ConfigOverrideUserPaths;
		public delegate m64p_error d_ConfigOverrideUserPaths(string var1, string var2);
	}
}
