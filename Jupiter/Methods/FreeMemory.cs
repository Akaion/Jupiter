using System;
using Jupiter.Etc;
using Jupiter.Services;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Methods
{
    internal static class FreeMemory
    {
        internal static bool Free(SafeProcessHandle processHandle, IntPtr baseAddress)
        {
            // Free memory in the remote process at the address
            
            var result = Native.VirtualFreeEx(processHandle, baseAddress, 0, (int) Native.MemoryAllocation.Release);
            
            if (!result)
            {
                ExceptionHandler.ThrowWin32Exception("Failed to protect memory in the remote process");
            }
            
            return result;
        }
    }
}