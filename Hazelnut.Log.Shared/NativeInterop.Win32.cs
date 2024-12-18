﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hazelnut.Log;

internal partial class NativeInterop
{
    private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
    private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

#if NETSTANDARD2_1_OR_GREATER
    [DllImport("kernel32")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    [DllImport("kernel32")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    [DllImport("kernel32", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    [DllImport("kernel32")]
    public static extern uint GetLastError();
#else
    [LibraryImport("kernel32", EntryPoint = "GetConsoleMode")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);
    [LibraryImport("kernel32", EntryPoint = "SetConsoleMode")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(nint hConsoleHandle, uint dwMode);
    [LibraryImport("kernel32", EntryPoint = "GetStdHandle", SetLastError = true)]
    private static partial nint GetStdHandle(int nStdHandle);
    [LibraryImport("kernel32", EntryPoint = "GetLastError")]
    private static partial uint GetLastError();
#endif

    public static bool EnableConsoleToAnsiEscapeSequence()
    {
        if (Environment.OSVersion.Version.Major < 10)
            return false;

        var iStdIn = GetStdHandle(STD_INPUT_HANDLE);
        var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

        if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
        {
            Debug.WriteLine("Failed to get stdin console mode: {0}", GetLastError());
            return false;
        }

        if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
        {
            Debug.WriteLine("Failed to get stdout console mode: {0}", GetLastError());
            return false;
        }

        inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;
        outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

        if (!SetConsoleMode(iStdIn, inConsoleMode))
        {
            Debug.WriteLine("Failed to set stdin console mode: {0}", GetLastError());
            return false;
        }

        if (!SetConsoleMode(iStdOut, outConsoleMode))
        {
            Debug.WriteLine("Failed to set stdout console mode: {0}", GetLastError());
            return false;
        }

        return true;
    }
}