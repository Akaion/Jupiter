using System;
using System.Runtime.InteropServices;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class ProtectMemory
    {
        internal static bool Protect(SafeHandle processHandle, IntPtr baseAddress, int size, int protection)
        {
            return VirtualProtectEx(processHandle, baseAddress, size, (MemoryProtection) protection, out _);
        }
    }
}