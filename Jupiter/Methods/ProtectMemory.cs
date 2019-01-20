using System;
using Jupiter.Etc;
using Jupiter.Services;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Methods
{
    internal static class ProtectMemory
    {
        internal static bool Protect(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int protection)
        {
            // Protect memory in the remote process at the address
            
            var result = Native.VirtualProtectEx(processHandle, baseAddress, size, protection, out _);
            
            if (!result)
            {
                ExceptionHandler.ThrowWin32Exception("Failed to protect memory in the remote process");
            }
            
            return result;
        }
    }
}