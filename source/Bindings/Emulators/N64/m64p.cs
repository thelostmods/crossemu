using System;
using System.Runtime.InteropServices;

using DynaLib_DotNet;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		private static IntPtr coreHandle;
		private static IntPtr audioHandle;
		private static IntPtr inputHandle;
		private static IntPtr rspHandle;
		private static IntPtr videoHandle;
		public static IntPtr CoreHandle => coreHandle;
		public static IntPtr AudioHandle => audioHandle;
		public static IntPtr InputHandle => inputHandle;
		public static IntPtr RspHandle => rspHandle;
		public static IntPtr VideoHandle => videoHandle;

		public static bool CoreIsLoaded => coreHandle != IntPtr.Zero;

		public static m64p_error LoadCore()
		{
			if (CoreIsLoaded) return m64p_error.SUCCESS;

			var lib = new DynaLib();
			if (!lib.LoadFromConfig("mupen64plus", "mupen64plus-core"))
			{
				Godot.GD.Print("There was a problem loading mupen64plus-core");
				return m64p_error.NOT_INIT;
			} coreHandle = lib.Handle;

			CoreBind(lib);

			return m64p_error.SUCCESS;
		}

		public static m64p_error LoadCore(string libDir = "", string depDir = "!")
		{
			if (CoreIsLoaded) return m64p_error.SUCCESS;

			var lib = new DynaLib();
			if (!lib.Load("mupen64plus", libDir, depDir))
			{
				Godot.GD.Print("There was a problem loading mupen64plus-core");
				return m64p_error.NOT_INIT;
			} coreHandle = lib.Handle;

			CoreBind(lib);

			return m64p_error.SUCCESS;
		}

		private static void CoreBind(DynaLib lib)
		{
			/* Common */
			lib.Bind(ref PluginGetVersion, "PluginGetVersion");
			lib.Bind(ref CoreGetAPIVersions, "CoreGetAPIVersions");
			lib.Bind(ref CoreErrorMessage, "CoreErrorMessage");

			/* Config */
			lib.Bind(ref ConfigListSections, "ConfigListSections");
			lib.Bind(ref ConfigOpenSection, "ConfigOpenSection");
			lib.Bind(ref ConfigListParameters, "ConfigListParameters");
			lib.Bind(ref ConfigSaveFile, "ConfigSaveFile");
			lib.Bind(ref ConfigSaveSection, "ConfigSaveSection");
			lib.Bind(ref ConfigHasUnsavedChanges, "ConfigHasUnsavedChanges");
			lib.Bind(ref ConfigDeleteSection, "ConfigDeleteSection");
			lib.Bind(ref ConfigRevertChanges, "ConfigRevertChanges");
			lib.Bind(ref ConfigSetParameter, "ConfigSetParameter");
			lib.Bind(ref ConfigSetParameterHelp, "ConfigSetParameterHelp");
			lib.Bind(ref ConfigGetParameter, "ConfigGetParameter");
			lib.Bind(ref ConfigGetParameterType, "ConfigGetParameterType");
			lib.Bind(ref ConfigGetParameterHelp, "ConfigGetParameterHelp");
			lib.Bind(ref ConfigSetDefaultInt, "ConfigSetDefaultInt");
			lib.Bind(ref ConfigSetDefaultFloat, "ConfigSetDefaultFloat");
			lib.Bind(ref ConfigSetDefaultBool, "ConfigSetDefaultBool");
			lib.Bind(ref ConfigSetDefaultString, "ConfigSetDefaultString");
			lib.Bind(ref ConfigGetParamInt, "ConfigGetParamInt");
			lib.Bind(ref ConfigGetParamFloat, "ConfigGetParamFloat");
			lib.Bind(ref ConfigGetParamBool, "ConfigGetParamBool");
			lib.Bind(ref ConfigGetParamString, "ConfigGetParamString");
			lib.Bind(ref ConfigGetSharedDataFilepath, "ConfigGetSharedDataFilepath");
			lib.Bind(ref ConfigGetUserConfigPath, "ConfigGetUserConfigPath");
			lib.Bind(ref ConfigGetUserDataPath, "ConfigGetUserDataPath");
			lib.Bind(ref ConfigGetUserCachePath, "ConfigGetUserCachePath");
			lib.Bind(ref ConfigExternalOpen, "ConfigExternalOpen");
			lib.Bind(ref ConfigExternalClose, "ConfigExternalClose");
			lib.Bind(ref ConfigExternalGetParameter, "ConfigExternalGetParameter");
			lib.Bind(ref ConfigSendNetplayConfig, "ConfigSendNetplayConfig");
			lib.Bind(ref ConfigReceiveNetplayConfig, "ConfigReceiveNetplayConfig");
			lib.Bind(ref ConfigOverrideUserPaths, "ConfigOverrideUserPaths");

			/* Debug */
			lib.Bind(ref DebugSetCallbacks, "DebugSetCallbacks");
			lib.Bind(ref DebugSetCoreCompare, "DebugSetCoreCompare");
			lib.Bind(ref DebugSetRunState, "DebugSetRunState");
			lib.Bind(ref DebugGetState, "DebugGetState");
			lib.Bind(ref DebugStep, "DebugStep");
			lib.Bind(ref DebugDecodeOp, "DebugDecodeOp");
			lib.Bind(ref DebugMemGetRecompInfo, "DebugMemGetRecompInfo");
			lib.Bind(ref DebugMemGetMemInfo, "DebugMemGetMemInfo");
			lib.Bind(ref DebugMemGetPointer, "DebugMemGetPointer");
			lib.Bind(ref DebugMemRead64, "DebugMemRead64");
			lib.Bind(ref DebugMemRead32, "DebugMemRead32");
			lib.Bind(ref DebugMemRead16, "DebugMemRead16");
			lib.Bind(ref DebugMemRead8, "DebugMemRead8");
			lib.Bind(ref DebugMemWrite64, "DebugMemWrite64");
			lib.Bind(ref DebugMemWrite32, "DebugMemWrite32");
			lib.Bind(ref DebugMemWrite16, "DebugMemWrite16");
			lib.Bind(ref DebugMemWrite8, "DebugMemWrite8");
			lib.Bind(ref DebugGetCPUDataPtr, "DebugGetCPUDataPtr");
			lib.Bind(ref DebugBreakpointLookup, "DebugBreakpointLookup");
			lib.Bind(ref DebugBreakpointCommand, "DebugBreakpointCommand");
			lib.Bind(ref DebugBreakpointTriggeredBy, "DebugBreakpointTriggeredBy");
			lib.Bind(ref DebugVirtualToPhysical, "DebugVirtualToPhysical");

			/* Front-End */
			lib.Bind(ref CoreStartup, "CoreStartup");
			lib.Bind(ref CoreShutdown, "CoreShutdown");
			lib.Bind(ref CoreAttachPlugin, "CoreAttachPlugin");
			lib.Bind(ref CoreDetachPlugin, "CoreDetachPlugin");
			lib.Bind(ref CoreDoCommand, "CoreDoCommand");
			lib.Bind(ref CoreOverrideVidExt, "CoreOverrideVidExt");
			lib.Bind(ref CoreAddCheat, "CoreAddCheat");
			lib.Bind(ref CoreCheatEnabled, "CoreCheatEnabled");
			lib.Bind(ref CoreGetRomSettings, "CoreGetRomSettings");

			if (
				lib.Bind(ref CoreSaveOverride, "CoreSaveOverride") &&
				lib.Bind(ref GetHeader, "GetHeader") &&
				lib.Bind(ref GetRdRam, "GetRdRam") &&
				lib.Bind(ref GetRom, "GetRom") &&
				lib.Bind(ref GetRdRamSize, "GetRdRamSize") &&
				lib.Bind(ref GetRomSize, "GetRomSize") &&
				lib.Bind(ref RefreshDynarec, "RefreshDynarec")
			){
				if (DynaLib_DotNet.Conditionals.DL_NOTIFICATION)
					Godot.GD.Print("Dynalib-Conditional: [M64P] Modding supported.");
				Conditionals.Modding = true;
			}
			else Conditionals.Modding = false;

			/* Plugin */

			/* VidExt */
			lib.Bind(ref VidExt_Init, "VidExt_Init");
			lib.Bind(ref VidExt_Quit, "VidExt_Quit");
			lib.Bind(ref VidExt_ListFullscreenModes, "VidExt_ListFullscreenModes");
			lib.Bind(ref VidExt_ListFullscreenRates, "VidExt_ListFullscreenRates");
			lib.Bind(ref VidExt_SetVideoMode, "VidExt_SetVideoMode");
			lib.Bind(ref VidExt_SetVideoModeWithRate, "VidExt_SetVideoModeWithRate");
			lib.Bind(ref VidExt_ResizeWindow, "VidExt_ResizeWindow");
			lib.Bind(ref VidExt_SetCaption, "VidExt_SetCaption");
			lib.Bind(ref VidExt_ToggleFullScreen, "VidExt_ToggleFullScreen");
			lib.Bind(ref VidExt_GL_GetProcAddress, "VidExt_GL_GetProcAddress");
			lib.Bind(ref VidExt_GL_SetAttribute, "VidExt_GL_SetAttribute");
			lib.Bind(ref VidExt_GL_GetAttribute, "VidExt_GL_GetAttribute");
			lib.Bind(ref VidExt_GL_SwapBuffers, "VidExt_GL_SwapBuffers");
			lib.Bind(ref VidExt_GL_GetDefaultFramebuffer, "VidExt_GL_GetDefaultFramebuffer");
		}
		
		public static m64p_error UnloadCore()
		{
			if (!CoreIsLoaded) return m64p_error.NOT_INIT;

			/* In case plugins are attached */
			DetachPlugins();

			Native.UnloadLib(coreHandle);
			coreHandle = IntPtr.Zero;

			/* Common */
			PluginGetVersion = null;
			CoreGetAPIVersions = null;
			CoreErrorMessage = null;
			PluginStartup = null;
			PluginShutdown = null;

			/* Config */
			ConfigListSections = null;
			ConfigOpenSection = null;
			ConfigListParameters = null;
			ConfigSaveFile = null;
			ConfigSaveSection = null;
			ConfigHasUnsavedChanges = null;
			ConfigDeleteSection = null;
			ConfigRevertChanges = null;
			ConfigSetParameter = null;
			ConfigSetParameterHelp = null;
			ConfigGetParameter = null;
			ConfigGetParameterType = null;
			ConfigGetParameterHelp = null;
			ConfigSetDefaultInt = null;
			ConfigSetDefaultFloat = null;
			ConfigSetDefaultBool = null;
			ConfigSetDefaultString = null;
			ConfigGetParamInt = null;
			ConfigGetParamFloat = null;
			ConfigGetParamBool = null;
			ConfigGetParamString = null;
			ConfigGetSharedDataFilepath = null;
			ConfigGetUserConfigPath = null;
			ConfigGetUserDataPath = null;
			ConfigGetUserCachePath = null;
			ConfigExternalOpen = null;
			ConfigExternalClose = null;
			ConfigExternalGetParameter = null;
			ConfigSendNetplayConfig = null;
			ConfigReceiveNetplayConfig = null;
			ConfigOverrideUserPaths = null;

			/* Debug */
			DebugSetCallbacks = null;
			DebugSetCoreCompare = null;
			DebugSetRunState = null;
			DebugGetState = null;
			DebugStep = null;
			DebugDecodeOp = null;
			DebugMemGetRecompInfo = null;
			DebugMemGetMemInfo = null;
			DebugMemGetPointer = null;
			DebugMemRead64 = null;
			DebugMemRead32 = null;
			DebugMemRead16 = null;
			DebugMemRead8 = null;
			DebugMemWrite64 = null;
			DebugMemWrite32 = null;
			DebugMemWrite16 = null;
			DebugMemWrite8 = null;
			DebugGetCPUDataPtr = null;
			DebugBreakpointLookup = null;
			DebugBreakpointCommand = null;
			DebugBreakpointTriggeredBy = null;
			DebugVirtualToPhysical = null;

			/* Front-End */
			CoreStartup = null;
			CoreShutdown = null;
			CoreAttachPlugin = null;
			CoreDetachPlugin = null;
			CoreDoCommand = null;
			CoreOverrideVidExt = null;
			CoreAddCheat = null;
			CoreCheatEnabled = null;
			if (Conditionals.Modding)
			{
				CoreSaveOverride = null;
				GetHeader = null;
				GetRdRam = null;
				GetRom = null;
				GetRdRamSize = null;
				GetRomSize = null;
				RefreshDynarec = null;
			}

			/* Plugin */

			/* VidExt */
			VidExt_Init = null;
			VidExt_Quit = null;
			VidExt_ListFullscreenModes = null;
			VidExt_ListFullscreenRates = null;
			VidExt_SetVideoMode = null;
			VidExt_SetVideoModeWithRate = null;
			VidExt_ResizeWindow = null;
			VidExt_SetCaption = null;
			VidExt_ToggleFullScreen = null;
			VidExt_GL_GetProcAddress = null;
			VidExt_GL_SetAttribute = null;
			VidExt_GL_GetAttribute = null;
			VidExt_GL_SwapBuffers = null;
			VidExt_GL_GetDefaultFramebuffer = null;

			return m64p_error.SUCCESS;
		}

		public static bool PluginIsLoaded(m64p_plugin_type type)
		{
			switch (type)
			{
				case m64p_plugin_type.CORE: return coreHandle != IntPtr.Zero;
				case m64p_plugin_type.AUDIO: return audioHandle != IntPtr.Zero;
				case m64p_plugin_type.INPUT: return inputHandle != IntPtr.Zero;
				case m64p_plugin_type.RSP: return rspHandle != IntPtr.Zero;
				case m64p_plugin_type.GFX: return videoHandle != IntPtr.Zero;
				default: return false;
			}
		}

		public static m64p_error AttachPlugins(DebugCallback debugCallback,
							   string videoPlugin, string audioPlugin,
							   string inputPlugin, string rspPlugin,
							   string libDir = "", string depDir = "!")
		{
			var success =
			(
			(LoadPlugin(ref videoHandle, m64p_plugin_type.GFX, debugCallback,
				libDir, videoPlugin, depDir) == m64p_error.SUCCESS) &&
			(LoadPlugin(ref audioHandle, m64p_plugin_type.AUDIO, debugCallback,
				libDir, audioPlugin, depDir) == m64p_error.SUCCESS) &&
			(LoadPlugin(ref inputHandle, m64p_plugin_type.INPUT, debugCallback,
				libDir, inputPlugin, depDir) == m64p_error.SUCCESS) &&
			(LoadPlugin(ref rspHandle, m64p_plugin_type.RSP, debugCallback,
				libDir, rspPlugin, depDir) == m64p_error.SUCCESS)
			);

			/* Successfully loaded all plugins */
			if (success) return m64p_error.SUCCESS;

			/* Failed horribly! Revert! */
			DetachPlugins();
			return m64p_error.PLUGIN_FAIL;
		}

		public static void DetachPlugins()
		{
			UnloadPlugin(ref rspHandle);
			UnloadPlugin(ref inputHandle);
			UnloadPlugin(ref audioHandle);
			UnloadPlugin(ref videoHandle);
		}

		private static m64p_error LoadPlugin(ref IntPtr plug, m64p_plugin_type type, DebugCallback callback,
						   string path, string name, string depsPath = "!")
		{
			/* Check if core is attached */
			if (!CoreIsLoaded) return m64p_error.NOT_INIT;

			/* Check if already attached */
			if (plug != IntPtr.Zero) return m64p_error.INVALID_STATE;

			/* Load the lib */
			var lib = new DynaLib(); lib.Load(name, path, depsPath);
			if (!lib.IsLoaded())
			{
				Godot.GD.Print("Mupen64Plus: failed to find " + name);
				return m64p_error.INPUT_NOT_FOUND;
			}

			/* Initialize the plugin */
			if (lib.Bind(ref PluginStartup, "PluginStartup"))
			{
				var context = Marshal.StringToHGlobalAnsi(name);
				PluginStartup!(coreHandle, context, callback);
				PluginStartup = null;
			} else return m64p_error.NOT_INIT;

			/* Attempt attaching it to core lib */
			var err = CoreAttachPlugin!(type, lib.Handle);
			if (err != m64p_error.SUCCESS)
			{
				lib.Unload();
				return err;
			}

			plug = lib.Handle;
			return m64p_error.SUCCESS;
		}

		private static m64p_error UnloadPlugin(ref IntPtr plug)
		{
			if (!CoreIsLoaded) return m64p_error.NOT_INIT;

			/* Check if not already attached */
			if (plug == IntPtr.Zero) return m64p_error.INVALID_STATE;

			/* Shutdown the plugin */
			Native.BindSymbol(plug, ref PluginShutdown, "PluginShutdown");
			PluginShutdown!();
			PluginShutdown = null;

			/* Detach the lib */
			Native.UnloadLib(plug);
			plug = IntPtr.Zero;

			return m64p_error.SUCCESS;
		}
	}
}
