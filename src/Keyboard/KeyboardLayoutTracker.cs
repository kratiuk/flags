using System.Runtime.InteropServices;

internal sealed class KeyboardLayoutTracker
{
    public bool TryGetCurrent(out KeyboardLayoutSnapshot snapshot)
    {
        var foregroundWindow = NativeMethods.GetForegroundWindow();
        if (foregroundWindow == IntPtr.Zero)
        {
            snapshot = default;
            return false;
        }

        var threadId = NativeMethods.GetWindowThreadProcessId(foregroundWindow, out _);
        if (threadId == 0)
        {
            snapshot = default;
            return false;
        }

        var keyboardLayout = NativeMethods.GetKeyboardLayout(threadId);
        if (keyboardLayout == IntPtr.Zero)
        {
            snapshot = default;
            return false;
        }

        var languageId = unchecked((ushort)(keyboardLayout.ToInt64() & 0xFFFF));
        snapshot = new KeyboardLayoutSnapshot(keyboardLayout, languageId);
        return true;
    }
}

internal readonly record struct KeyboardLayoutSnapshot(IntPtr Handle, ushort LanguageId);

internal static partial class NativeMethods
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    public static extern IntPtr GetKeyboardLayout(uint idThread);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyIcon(IntPtr hIcon);
}
