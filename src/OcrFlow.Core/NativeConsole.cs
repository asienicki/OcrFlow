using System.Runtime.InteropServices;

public static class NativeConsole
{
    [DllImport("kernel32.dll")]
    static extern bool SetStdHandle(int nStdHandle, IntPtr handle);

    const int STD_ERROR_HANDLE = -12;

    public static void MuteStderr()
        => SetStdHandle(STD_ERROR_HANDLE, IntPtr.Zero);
}