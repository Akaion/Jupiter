using System;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class AllocateMemory
    {
        internal static IntPtr Allocate(SafeProcessHandle processHandle, int size)
        {
            // Allocate memory in the process

            const MemoryAllocation allocationType = MemoryAllocation.Commit | MemoryAllocation.Reserve;

            return VirtualAllocEx(processHandle, IntPtr.Zero, size, (int) allocationType, (int) MemoryProtection.PageExecuteReadWrite);
        }
    }
}