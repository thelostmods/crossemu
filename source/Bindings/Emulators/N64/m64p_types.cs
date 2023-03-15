using System;

namespace Mupen64Plus_DotNet
{
	/* ----------------------------------------- */
	/* Structures and Types for Core library API */
	/* ----------------------------------------- */

	public delegate void m64p_function();
	public delegate void m64p_frame_callback(uint FrameIndex);
	public delegate void m64p_input_callback();
	public delegate void m64p_audio_callback();
	public delegate void m64p_vi_callback();

	public enum m64p_type : int
	{
		INT = 1,
		FLOAT,
		BOOL,
		STRING
	}

	public enum m64p_msg_level : int
	{
		ERROR = 1,
		WARNING,
		INFO,
		STATUS,
		VERBOSE
	}

	public enum m64p_error : int
	{
		SUCCESS = 0,
		NOT_INIT,        /* Function is disallowed before InitMupen64Plus() is called */
		ALREADY_INIT,    /* InitMupen64Plus() was called twice */
		INCOMPATIBLE,    /* API versions between components are incompatible */
		INPUT_ASSERT,    /* Invalid parameters for function call, such as ParamValue=NULL for GetCoreParameter() */
		INPUT_INVALID,   /* Invalid input data, such as ParamValue="maybe" for SetCoreParameter() to set a BOOL-type value */
		INPUT_NOT_FOUND, /* The input parameter(s) specified a particular item which was not found */
		NO_MEMORY,       /* Memory allocation failed */
		FILES,           /* Error opening, creating, reading, or writing to a file */
		INTERNAL,        /* Internal error (bug) */
		INVALID_STATE,   /* Current program state does not allow operation */
		PLUGIN_FAIL,     /* A plugin function returned a fatal error */
		SYSTEM_FAIL,     /* A system function call, such as an SDL or file operation, failed */
		UNSUPPORTED,     /* Function call is not supported (ie, core not built with debugger) */
		WRONG_TYPE       /* A given input type parameter cannot be used for desired operation */
	}

	public enum m64p_core_caps : int
	{
		DYNAREC = 1,
		DEBUGGER = 2,
		CORE_COMPARE = 4
	}

	public enum m64p_plugin_type : int
	{
		NULL = 0,
		RSP = 1,
		GFX,
		AUDIO,
		INPUT,
		CORE
	}

	public enum m64p_emu_state : int
	{
		STOPPED = 1,
		RUNNING,
		PAUSED
	}

	public enum m64p_video_mode : int
	{
		NONE = 1,
		WINDOWED,
		FULLSCREEN
	}

	public enum m64p_video_flags : int
	{
		SUPPORT_RESIZING = 1
	}

	public enum m64p_core_param : int
	{
		EMU_STATE = 1,
		VIDEO_MODE,
		SAVESTATE_SLOT,
		SPEED_FACTOR,
		SPEED_LIMITER,
		VIDEO_SIZE,
		AUDIO_VOLUME,
		AUDIO_MUTE,
		INPUT_GAMESHARK,
		STATE_LOADCOMPLETE,
		STATE_SAVECOMPLETE
	}

	public enum m64p_command : int
	{
		NOP = 0,
		ROM_OPEN,
		ROM_CLOSE,
		ROM_GET_HEADER,
		ROM_GET_SETTINGS,
		EXECUTE,
		STOP,
		PAUSE,
		RESUME,
		CORE_STATE_QUERY,
		STATE_LOAD,
		STATE_SAVE,
		STATE_SET_SLOT,
		SEND_SDL_KEYDOWN,
		SEND_SDL_KEYUP,
		SET_FRAME_CALLBACK,
		TAKE_NEXT_SCREENSHOT,
		CORE_STATE_SET,
		READ_SCREEN,
		RESET,
		ADVANCE_FRAME,
		SET_MEDIA_LOADER,
		NETPLAY_INIT,
		NETPLAY_CONTROL_PLAYER,
		NETPLAY_GET_VERSION,
		NETPLAY_CLOSE,
		PIF_OPEN,
		ROM_SET_SETTINGS
	}

	public struct m64p_cheat_code
	{
		public uint address;
		public int value;
	}

	public struct m64p_media_loader
	{

		/// <summary> Frontend-defined callback data. </summary>
		public d_cb_data? cb_data;

		/// <summary>
		/// Allow the frontend to specify the GB cart ROM file to load
		/// cb_data: points to frontend-defined callback data.
		/// controller_num: (0-3) tell the frontend which controller is about to load a GB cart
		/// Returns a NULL-terminated string owned by the core specifying the GB cart ROM filename to load.
		/// Empty or NULL string results in no GB cart being loaded (eg. empty transferpak).
		/// </summary>
		public d_get_gb_cart_rom? get_gb_cart_rom;

		/// <summary>
		/// Allow the frontend to specify the GB cart RAM file to load
		/// cb_data: points to frontend-defined callback data.
		/// controller_num: (0-3) tell the frontend which controller is about to load a GB cart
		/// Returns a NULL-terminated string owned by the core specifying the GB cart RAM filename to load
		/// Empty or NULL string results in the core generating a default save file with empty content.
		/// </summary>
		public d_get_gb_cart_ram? get_gb_cart_ram;

		/// <summary>
		/// Allow the frontend to specify the DD IPL ROM file to load
		/// cb_data: points to frontend-defined callback data.
		/// Returns a NULL-terminated string owned by the core specifying the DD IPL ROM filename to load
		/// Empty or NULL string results in disabled 64DD.
		/// </summary>
		public d_get_dd_rom? get_dd_rom;

		/// <summary>
		/// Allow the frontend to specify the DD disk file to load
		/// cb_data: points to frontend-defined callback data.
		/// Returns a NULL-terminated string owned by the core specifying the DD disk filename to load
		/// Empty or NULL string results in no DD disk being loaded (eg. empty disk drive).
		/// </summary>
		public d_get_dd_disk? get_dd_disk;


		public delegate IntPtr d_cb_data();
		public delegate string d_get_gb_cart_rom(IntPtr cb_data, int controller_num);
		public delegate string d_get_gb_cart_ram(IntPtr cb_data, int controller_num);
		public delegate string d_get_dd_rom(IntPtr cb_data);
		public delegate string d_get_dd_disk(IntPtr cb_data);
	}

	/* ----------------------------------------- */
	/* Structures to hold ROM image information  */
	/* ----------------------------------------- */

	public enum m64p_system_type : int
	{
		NTSC = 0,
		PAL,
		MPAL
	}

	public enum m64p_rom_save_type : int
	{
		EEPROM_4KB = 0,
		EEPROM_16KB,
		SRAM,
		FLASH_RAM,
		CONTROLLER_PACK,
		NONE
	}

	public unsafe struct m64p_rom_header
	{
		public byte init_PI_BSB_DOM1_LAT_REG;  /* 0x00 */
		public byte init_PI_BSB_DOM1_PGS_REG;  /* 0x01 */
		public byte init_PI_BSB_DOM1_PWD_REG;  /* 0x02 */
		public byte init_PI_BSB_DOM1_PGS_REG2; /* 0x03 */
		public uint ClockRate;                 /* 0x04 */
		public uint PC;                        /* 0x08 */
		public uint Release;                   /* 0x0C */
		public uint CRC1;                      /* 0x10 */
		public uint CRC2;                      /* 0x14 */
		public fixed uint Unknown[2];          /* 0x18 */
		public fixed char Name[20];            /* 0x20 */
		public uint unknown;                   /* 0x34 */
		public uint Manufacturer_ID;           /* 0x38 */
		public ushort Cartridge_ID;            /* 0x3C - Game serial number  */
		public ushort Country_code;            /* 0x3E */
	}

	public unsafe struct m64p_rom_settings
	{
		public fixed char goodname[256];
		public fixed char MD5[33];
		public byte savetype;
		public byte status;          /* Rom status on a scale from 0-5. */
		public byte players;         /* Local players 0-4, 2/3/4 way Netplay indicated by 5/6/7. */
		public byte rumble;          /* 0 - No, 1 - Yes boolean for rumble support. */
		public byte transferpak;     /* 0 - No, 1 - Yes boolean for transfer pak support. */
		public byte mempak;          /* 0 - No, 1 - Yes boolean for memory pak support. */
		public byte biopak;          /* 0 - No, 1 - Yes boolean for bio pak support. */
		public byte disableextramem; /* 0 - No, 1 - Yes boolean for disabling 4MB expansion RAM pack */
		public uint countperop;      /* Number of CPU cycles per instruction. */
		public uint sidmaduration;   /* Default SI DMA duration */
	}

	/* ----------------------------------------- */
	/* Structures and Types for the Debugger     */
	/* ----------------------------------------- */

	public enum m64p_dbg_state
	{
		RUN_STATE = 1,
		PREVIOUS_PC,
		NUM_BREAKPOINTS,
		CPU_DYNACORE,
		CPU_NEXT_INTERRUPT
	}

	public enum m64p_dbg_runstate
	{
		RUNSTATE_PAUSED = 0,
		RUNSTATE_STEPPING,
		RUNSTATE_RUNNING
	}

	public enum m64p_dbg_mem_info
	{
		MEM_TYPE = 1,
		MEM_FLAGS,
		MEM_HAS_RECOMPILED,
		MEM_NUM_RECOMPILED,
		RECOMP_OPCODE = 16,
		RECOMP_ARGS,
		RECOMP_ADDR
	}

	public enum m64p_dbg_mem_type
	{
		NOMEM = 0,
		NOTHING,
		RDRAM,
		RDRAMREG,
		RSPMEM,
		RSPREG,
		RSP,
		DP,
		DPS,
		VI,
		AI,
		PI,
		RI,
		SI,
		FLASHRAMSTAT,
		ROM,
		PIF,
		MI,
		BREAKPOINT
	}

	public enum m64p_dbg_mem_flags
	{
		READABLE = 0x01,
		WRITABLE = 0x02,
		READABLE_EMUONLY = 0x04,  /* the EMUONLY flags signify that emulated code can read/write here, but debugger cannot */
		WRITABLE_EMUONLY = 0x08
	}

	public enum m64p_dbg_memptr_type
	{
		RDRAM = 1,
		PI_REG,
		SI_REG,
		VI_REG,
		RI_REG,
		AI_REG
	}

	public enum m64p_dbg_cpu_data
	{
		PC = 1,
		REG_REG,
		REG_HI,
		REG_LO,
		REG_COP0,
		REG_COP1_DOUBLE_PTR,
		REG_COP1_SIMPLE_PTR,
		REG_COP1_FGR_64,
		TLB
	}

	public enum m64p_dbg_bkp_command
	{
		ADD_ADDR = 1,
		ADD_STRUCT,
		REPLACE,
		REMOVE_ADDR,
		REMOVE_IDX,
		ENABLE,
		DISABLE,
		CHECK
	}

	//enum M64P_MEM_INVALID = 0xFFFFFFFF;  /* invalid memory read will return this */

	//enum BREAKPOINTS_MAX_NUMBER = 128;

	public enum m64p_dbg_bkp_flags
	{
		ENABLED = 0x01,
		READ = 0x02,
		WRITE = 0x04,
		EXEC = 0x08,
		LOG = 0x10 /* Log to the console when this breakpoint hits */
	}

	//bool BPT_CHECK_FLAG(T)(ref m64p_breakpoint a, T b) if (isNumeric!T) { return (a.flags & b) == b; }
	//void BPT_SET_FLAG(T)(ref m64p_breakpoint a, T b) if (isNumeric!T) { a.flags = (a.flags | b); }
	//void BPT_CLEAR_FLAG(T)(ref m64p_breakpoint a, T b) if (isNumeric!T) { a.flags = (a.flags & (~b)); }
	//void BPT_TOGGLE_FLAG(T)(ref m64p_breakpoint a, T b) if (isNumeric!T) { a.flags = (a.flags ^ b); }

	public struct m64p_breakpoint
	{
		public uint address;
		public uint endaddr;
		public uint flags;
	}

	/* ------------------------------------------------- */
	/* Structures and Types for Core Video Extension API */
	/* ------------------------------------------------- */

	public struct m64p_2d_size
	{
		public uint uiWidth;
		public uint uiHeight;
	}

	public enum m64p_GLattr
	{
		DOUBLEBUFFER = 1,
		BUFFER_SIZE,
		DEPTH_SIZE,
		RED_SIZE,
		GREEN_SIZE,
		BLUE_SIZE,
		ALPHA_SIZE,
		SWAP_CONTROL,
		MULTISAMPLEBUFFERS,
		MULTISAMPLESAMPLES,
		CONTEXT_MAJOR_VERSION,
		CONTEXT_MINOR_VERSION,
		CONTEXT_PROFILE_MASK
	}

	public enum m64p_GLContextType
	{
		CORE,
		COMPATIBILITY,
		ES
	}

	public struct m64p_video_extension_functions
	{
		public uint Functions;
		public d_VidExtFuncInit? VidExtFuncInit;
		public d_VidExtFuncQuit? VidExtFuncQuit;
		public d_VidExtFuncListModes? VidExtFuncListModes;
		public d_VidExtFuncListRates? VidExtFuncListRates;
		public d_VidExtFuncSetMode? VidExtFuncSetMode;
		public d_VidExtFuncSetModeWithRate? VidExtFuncSetModeWithRate;
		public d_VidExtFuncGLGetProc? VidExtFuncGLGetProc;
		public d_VidExtFuncGLSetAttr? VidExtFuncGLSetAttr;
		public d_VidExtFuncGLGetAttr? VidExtFuncGLGetAttr;
		public d_VidExtFuncGLSwapBuf? VidExtFuncGLSwapBuf;
		public d_VidExtFuncSetCaption? VidExtFuncSetCaption;
		public d_VidExtFuncToggleFS? VidExtFuncToggleFS;
		public d_VidExtFuncResizeWindow? VidExtFuncResizeWindow;
		public d_VidExtFuncGLGetDefaultFramebuffer? VidExtFuncGLGetDefaultFramebuffer;

		public delegate m64p_error d_VidExtFuncInit();
		public delegate m64p_error d_VidExtFuncQuit();
		public delegate m64p_error d_VidExtFuncListModes(ref m64p_2d_size var1, ref int var2);
		public delegate m64p_error d_VidExtFuncListRates(m64p_2d_size var1, ref int var2, ref int var3);
		public delegate m64p_error d_VidExtFuncSetMode(int var1, int var2, int var3, int var4, int var5);
		public delegate m64p_error d_VidExtFuncSetModeWithRate(int var1, int var2, int var3, int var4, int var5, int var6);
		public delegate m64p_function d_VidExtFuncGLGetProc(string var1);
		public delegate m64p_error d_VidExtFuncGLSetAttr(m64p_GLattr var1, int var2);
		public delegate m64p_error d_VidExtFuncGLGetAttr(m64p_GLattr var1, ref int var2);
		public delegate m64p_error d_VidExtFuncGLSwapBuf();
		public delegate m64p_error d_VidExtFuncSetCaption(string var1);
		public delegate m64p_error d_VidExtFuncToggleFS();
		public delegate m64p_error d_VidExtFuncResizeWindow(int var1, int var2);
		public delegate uint d_VidExtFuncGLGetDefaultFramebuffer();
	}
}
