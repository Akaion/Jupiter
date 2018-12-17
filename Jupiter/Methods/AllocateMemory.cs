using System;
using System.Runtime.InteropServices;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class AllocateMemory
    {
        internal static IntPtr Allocate(SafeHandle processHandle, int size)
        {
            // Allocate memory in the process

            const MemoryAllocation allocationType = MemoryAllocation.Commit | MemoryAllocation.Reserve;

            return VirtualAllocEx(processHandle, IntPtr.Zero, size, allocationType, MemoryProtection.PageExecuteReadWrite);
        }
    }
}