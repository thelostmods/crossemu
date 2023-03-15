using System;

namespace Mupen64Plus_DotNet
{
	public static partial class M64P
	{
		/// <summary>
		/// DebugSetCallbacks()
		///
		/// This function is called by the front-end to supply debugger callback
		/// function pointers.If debugger is enabled and then later disabled within the
		/// UI, this function may be called with NULL pointers in order to disable the
		/// callbacks.
		/// </summary>
		public static d_DebugSetCallbacks? DebugSetCallbacks;
		public delegate m64p_error d_DebugSetCallbacks(d1_DebugSetCallbacks? var1, d2_DebugSetCallbacks? var2, d3_DebugSetCallbacks? var3);
		public delegate void d1_DebugSetCallbacks();
		public delegate void d2_DebugSetCallbacks(uint var1);
		public delegate void d3_DebugSetCallbacks();

		/// <summary>
		/// DebugSetCoreCompare()
		///
		/// This function is called by the front-end to supply callback function pointers
		/// for the Core Comparison feature.
		/// </summary>
		public static d_DebugSetCoreCompare? DebugSetCoreCompare;
		public delegate m64p_error d_DebugSetCoreCompare(d1_DebugSetCoreCompare? var1, d2_DebugSetCoreCompare? var2);
		public delegate void d1_DebugSetCoreCompare(uint var1);
		public delegate void d2_DebugSetCoreCompare(uint var1, IntPtr var2);

		/// <summary>
		/// DebugSetRunState()
		///
		/// This function sets the run state of the R4300 CPU emulator.
		/// </summary>
		public static d_DebugSetRunState? DebugSetRunState;
		public delegate m64p_error d_DebugSetRunState(m64p_dbg_runstate var1);

		/// <summary>
		/// DebugGetState()
		///
		/// This function reads and returns a debugger state variable, which are
		/// enumerated in m64p_types.h.
		/// </summary>
		public static d_DebugGetState? DebugGetState;
		public delegate int d_DebugGetState(m64p_dbg_runstate var1);

		/// <summary>
		/// DebugStep()
		///
		/// This function signals the debugger to advance one instruction when in the
		/// stepping mode.
		/// </summary>
		public static d_DebugStep? DebugStep;
		public delegate m64p_error d_DebugStep();


		/// <summary>
		/// DebugDecodeOp()
		///
		/// This is a helper function for the debugger front-end.This instruction takes
		/// a PC value and an R4300 instruction opcode and writes the disassembled
		/// instruction mnemonic and arguments into character buffers.This is intended
		/// to be used to display disassembled code. 
		/// </summary>
		public static d_DebugDecodeOp? DebugDecodeOp;
		public delegate void d_DebugDecodeOp(uint var1, ref string var2, ref string var3, int var4);

		/// <summary>
		/// DebugMemGetRecompInfo()
		///
		/// This function is used by the front-end to retrieve disassembly information
		/// about recompiled code.For example, the dynamic recompiler may take a single
		/// R4300 instruction and compile it into 10 x86 instructions. This function may
		/// then be used to retrieve the disassembled code of the 10 x86 instructions.
		/// </summary>
		public static d_DebugMemGetRecompInfo? DebugMemGetRecompInfo;
		public delegate IntPtr d_DebugMemGetRecompInfo(m64p_dbg_mem_info var1, uint var2, int var3);

		/// <summary>
		/// DebugMemGetMemInfo()
		///
		/// This function returns an integer value regarding the memory location address,
		/// corresponding to the information requested by mem_info_type, which is a type
		/// enumerated in m64p_types.h.
		/// </summary>
		public static d_DebugMemGetMemInfo? DebugMemGetMemInfo;
		public delegate int d_DebugMemGetMemInfo(m64p_dbg_mem_info var1, uint var2);

		/// <summary>
		/// DebugMemGetPointer()
		///
		/// This function returns a memory pointer(in x86 memory space) to a block of
		/// emulated N64 memory.This may be used to retrieve a pointer to a special N64
		/// block(such as the serial, video, or audio registers) or the RDRAM.
		/// </summary>
		public static d_DebugMemGetPointer? DebugMemGetPointer;
		public delegate IntPtr d_DebugMemGetPointer(m64p_dbg_mem_info var1);

		///
		/// DebugMemRead**()
		///
		/// These functions retrieve a value from the emulated N64 memory.The returned
		/// value will be correctly byte-swapped for the host architecture.
		///
		public static d_DebugMemRead64? DebugMemRead64;
		public static d_DebugMemRead32? DebugMemRead32;
		public static d_DebugMemRead16? DebugMemRead16;
		public static d_DebugMemRead8? DebugMemRead8;
		
		public delegate ulong d_DebugMemRead64(uint var1);
		public delegate uint d_DebugMemRead32(uint var1);
		public delegate ushort d_DebugMemRead16(uint var1);
		public delegate byte d_DebugMemRead8(uint var1);


		///
		/// DebugMemWrite**()
		///
		/// These functions write a value into the emulated N64 memory.The given value
		/// will be correctly byte-swapped before storage.
		///
		public static d_DebugMemWrite64? DebugMemWrite64;
		public static d_DebugMemWrite32? DebugMemWrite32;
		public static d_DebugMemWrite16? DebugMemWrite16;
		public static d_DebugMemWrite8? DebugMemWrite8;
		
		public delegate void d_DebugMemWrite64(uint var1, ulong var2);
		public delegate void d_DebugMemWrite32(uint var1, uint var2);
		public delegate void d_DebugMemWrite16(uint var1, ushort var2);
		public delegate void d_DebugMemWrite8(uint var1, byte var2);

		/// <summary> DebugGetCPUDataPtr()
		///
		/// This function returns a memory pointer(in x86 memory space) to a specific
		/// register in the emulated R4300 CPU.
		/// </summary>
		public static d_DebugGetCPUDataPtr? DebugGetCPUDataPtr;
		public delegate IntPtr d_DebugGetCPUDataPtr(m64p_dbg_cpu_data var1);

		/// <summary>
		/// DebugBreakpointLookup()
		///
		/// This function searches through all current breakpoints in the debugger to
		/// find one that matches the given input parameters.If a matching breakpoint
		/// is found, the index number is returned.If no breakpoints are found, -1 is
		/// returned.
		/// </summary>
		public static d_DebugBreakpointLookup? DebugBreakpointLookup;
		public delegate int d_DebugBreakpointLookup(uint var1, uint var2, uint var3);

		/// <summary>
		/// DebugBreakpointCommand()
		///
		/// This function is used to process common breakpoint commands, such as adding,
		/// removing, or searching the breakpoints.The meanings of the index and ptr
		/// input parameters vary by command.
		/// </summary>
		public static d_DebugBreakpointCommand? DebugBreakpointCommand;
		public delegate int d_DebugBreakpointCommand(m64p_dbg_bkp_command var1, uint var2, ref m64p_breakpoint var3);

		/// <summary>
		/// DebugBreakpointTriggeredBy()
		///
		/// This function is used to retrieve the trigger flags and address for the
		/// most recently triggered breakpoint.
		/// </summary>
		public static d_DebugBreakpointTriggeredBy? DebugBreakpointTriggeredBy;
		public delegate void d_DebugBreakpointTriggeredBy(ref uint var1, ref uint var2);

		/// <summary>
		/// DebugVirtualToPhysical()
		///
		/// This function is used to translate virtual addresses to physical addresses.
		/// Memory read/write breakpoints operate on physical addresses.
		/// </summary>
		public static d_DebugVirtualToPhysical? DebugVirtualToPhysical;
		public delegate uint d_DebugVirtualToPhysical(uint var1);
	}
}
