using System;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class ProtectMemory
    {
        internal static bool Protect(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int protection)
        {
            return VirtualProtectEx(processHandle, baseAddress, size, (MemoryProtection) protection, out _);
        }
    }
}