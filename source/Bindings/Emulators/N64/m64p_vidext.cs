using System;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		/// <summary>
		/// VidExt_Init()
		///
		/// This function should be called from within the InitiateGFX() video plugin
		/// function call.The default SDL implementation of this function simply calls
		/// SDL_InitSubSystem(SDL_INIT_VIDEO). It does not open a rendering window or
		/// switch video modes.
		/// </summary>
		public static d_VidExt_Init? VidExt_Init;
		public delegate m64p_error d_VidExt_Init();

		/// <summary>
		/// VidExt_Quit()
		///
		/// This function closes any open rendering window and shuts down the video
		/// system.The default SDL implementation of this function calls
		/// SDL_QuitSubSystem(SDL_INIT_VIDEO). This function should be called from
		/// within the RomClose() video plugin function.
		/// </summary>
		public static d_VidExt_Quit? VidExt_Quit;
		public delegate m64p_error d_VidExt_Quit();

		/// <summary>
		/// VidExt_ListFullscreenModes()
		///
		/// This function is used to enumerate the available resolutions for fullscreen
		/// video modes. A pointer to an array is passed into the function, which is
		/// then filled with resolution sizes.
		/// </summary>
		public static d_VidExt_ListFullscreenModes? VidExt_ListFullscreenModes;
		public delegate m64p_error d_VidExt_ListFullscreenModes(ref m64p_2d_size var1, ref int var2);

		/// <summary>
		/// VidExt_ListFullscreenRates()
		///
		/// This function is used to enumerate the available refresh rates for a fullscreen
		/// video mode.
		/// </summary>
		public static d_VidExt_ListFullscreenRates? VidExt_ListFullscreenRates;
		public delegate m64p_error d_VidExt_ListFullscreenRates(m64p_2d_size var1, ref int var2, ref int var3);

		/// <summary>
		/// VidExt_SetVideoMode()
		///
		/// This function creates a rendering window or switches into a fullscreen
		/// video mode.Any desired OpenGL attributes should be set before calling
		/// this function.
		/// </summary>
		public static d_VidExt_SetVideoMode? VidExt_SetVideoMode;
		public delegate m64p_error d_VidExt_SetVideoMode(int var1, int var2, int var3, m64p_video_mode var4, m64p_video_flags var5);

		/// <summary>
		/// VidExt_SetVideoModeWithRate()
		///
		/// This function creates a rendering window or switches into a fullscreen
		/// video mode. Any desired OpenGL attributes should be set before calling
		/// this function.
		/// </summary>
		public static d_VidExt_SetVideoModeWithRate? VidExt_SetVideoModeWithRate;
		public delegate m64p_error d_VidExt_SetVideoModeWithRate(int var1, int var2, int var3, int var4, m64p_video_mode var5, m64p_video_flags var6);

		/// <summary>
		/// VidExt_ResizeWindow()
		///
		/// This function resizes the opengl rendering window to match the given size.
		/// </summary>
		public static d_VidExt_ResizeWindow? VidExt_ResizeWindow;
		public delegate m64p_error d_VidExt_ResizeWindow(int var1, int var2);

		/// <summary>
		/// VidExt_SetCaption()
		///
		/// This function sets the caption text of the emulator rendering window.
		/// </summary>
		public static d_VidExt_SetCaption? VidExt_SetCaption;
		public delegate m64p_error d_VidExt_SetCaption(string var1);

		/// <summary>
		/// VidExt_ToggleFullScreen()
		///
		/// This function toggles between fullscreen and windowed rendering modes.
		/// </summary>
		public static d_VidExt_ToggleFullScreen? VidExt_ToggleFullScreen;
		public delegate m64p_error d_VidExt_ToggleFullScreen();

		/// <summary>
		/// VidExt_GL_GetProcAddress()
		///
		/// This function is used to get a pointer to an OpenGL extension function.This
		/// is only necessary on the Windows platform, because the OpenGL implementation
		/// shipped with Windows only supports OpenGL version 1.1.
		/// </summary>
		public static d_VidExt_GL_GetProcAddress? VidExt_GL_GetProcAddress;
		public delegate m64p_error d_VidExt_GL_GetProcAddress(string var1);

		/// <summary>
		/// VidExt_GL_SetAttribute()
		///
		/// This function is used to set certain OpenGL attributes which must be
		/// specified before creating the rendering window with VidExt_SetVideoMode.
		/// </summary>
		public static d_VidExt_GL_SetAttribute? VidExt_GL_SetAttribute;
		public delegate m64p_error d_VidExt_GL_SetAttribute(m64p_GLattr var1, int var2);

		/// <summary>
		/// VidExt_GL_GetAttribute()
		///
		/// This function is used to get the value of OpenGL attributes.These values may
		/// be changed when calling VidExt_SetVideoMode.
		/// </summary>
		public static d_VidExt_GL_GetAttribute? VidExt_GL_GetAttribute;
		public delegate m64p_error d_VidExt_GL_GetAttribute(m64p_GLattr var1, ref int var2);

		/// <summary>
		/// VidExt_GL_SwapBuffers()
		///
		/// This function is used to swap the front/back buffers after rendering an
		/// output video frame.
		/// </summary>
		public static d_VidExt_GL_SwapBuffers? VidExt_GL_SwapBuffers;
		public delegate m64p_error d_VidExt_GL_SwapBuffers();

		/// <summary>
		/// VidExt_GL_GetDefaultFramebuffer()
		///
		/// On some platforms(for instance, iOS) the default framebuffer object
		/// depends on the surface being rendered to, and might be different from 0.
		/// This function should be called after VidExt_SetVideoMode to retrieve the
		/// name of the default FBO.
		/// Calling this function may have performance implications
		/// and it should not be called every time the default FBO is bound.
		/// </summary>
		public static d_VidExt_GL_GetDefaultFramebuffer? VidExt_GL_GetDefaultFramebuffer;
		public delegate uint d_VidExt_GL_GetDefaultFramebuffer();
	}
}
