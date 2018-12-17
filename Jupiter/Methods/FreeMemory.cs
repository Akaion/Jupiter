using System;
using System.Runtime.InteropServices;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class FreeMemory
    {
        internal static bool Free(SafeHandle processHandle, IntPtr baseAddress, int size)
        {
            // Free memory in the process at the address
            
            return VirtualFreeEx(processHandle, baseAddress, size, MemoryAllocation.Release);
        }
    }
}