using System;
using System.Runtime.InteropServices;

public static class JSBSimNative
{
    private const string DllName = "JSBSimUnity";

    [DllImport(DllName)]
    public static extern bool JSB_Init(string rootDir, string aircraftPath, string enginePath, string systemsPath);

    [DllImport(DllName)]
    public static extern bool JSB_LoadAircraft(string aircraftName);

    [DllImport(DllName)]
    public static extern bool JSB_LoadScript(string scriptPath);

    [DllImport(DllName)]
    public static extern bool JSB_RunStep();

    [DllImport(DllName)]
    public static extern bool JSB_RunInitialConditions();

    [DllImport(DllName)]
    public static extern double JSB_GetProperty(string propertyName);

    [DllImport(DllName)]
    public static extern bool JSB_SetProperty(string propertyName, double value);

    [DllImport(DllName)]
    private static extern IntPtr JSB_GetLastError();

    [DllImport(DllName)]
    public static extern void JSB_Shutdown();

    public static string GetLastError()
    {
        var ptr = JSB_GetLastError();
        return ptr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(ptr);
    }
}