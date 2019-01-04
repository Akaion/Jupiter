using System;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class ProtectMemory
    {
        internal static bool Protect(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int protection)
        {
            // Protect memory in the process at the address
            
            return VirtualProtectEx(processHandle, baseAddress, size, protection, out _);
        }
    }
}