using System;
using System.Threading;

internal partial class Program
{
	public static Console ActiveConsole = new Console();
	public static SettingsDef Settings = new();

	public static Thread? EmulatorThread;

    public static bool UI_Hidden;

	/// <summary>
	/// Force GC cleanup
	/// </summary>
	public static void CollectGarbage()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}
}
