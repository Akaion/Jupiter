using System;
using Jupiter.Etc;
using Jupiter.Services;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Methods
{
    internal static class AllocateMemory
    {
        internal static IntPtr Allocate(SafeProcessHandle processHandle, int size)
        {
            // Allocate memory in the process

            const Native.MemoryAllocation allocationType = Native.MemoryAllocation.Commit | Native.MemoryAllocation.Reserve;

            var result = Native.VirtualAllocEx(processHandle, IntPtr.Zero, size, (int) allocationType, (int) Native.MemoryProtection.PageExecuteReadWrite);

            if (result == IntPtr.Zero)
            {
                ExceptionHandler.ThrowWin32Exception("Failed to allocate memory in the process");
            }

            return result;
        }
    }
}