using System;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class FreeMemory
    {
        internal static bool Free(SafeProcessHandle processHandle, IntPtr baseAddress)
        {
            // Free memory in the process at the address
            
            return VirtualFreeEx(processHandle, baseAddress, 0, (int) MemoryAllocation.Release);
        }
    }
}