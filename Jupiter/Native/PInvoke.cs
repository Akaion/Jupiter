using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Native.Enumerations;
using static Jupiter.Native.Structures;

namespace Jupiter.Native
{
    internal static class PInvoke
    {
        // kernel32.dll imports
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, IntPtr bytesReadBuffer, int bytesToRead, IntPtr numberOfBytesReadBuffer);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualAllocEx(SafeProcessHandle processHandle, IntPtr baseAddress, int allocationSize, AllocationType allocationType, MemoryProtection protectionType);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFreeEx(SafeProcessHandle processHandle, IntPtr baseAddress, int freeSize, FreeType freeType);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtectEx(SafeProcessHandle processHandle, IntPtr baseAddress, int protectionSize, MemoryProtection protectionType, out MemoryProtection oldProtectionType);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualQueryEx(SafeProcessHandle processHandle, IntPtr baseAddress, out MemoryBasicInformation memoryInformation, int length);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, IntPtr bufferToWrite, int bytesToWriteSize, IntPtr numberOfBytesWrittenBuffer);
    }
}