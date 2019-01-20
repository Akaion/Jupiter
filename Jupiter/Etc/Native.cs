using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Etc
{
    internal static class Native
    {
        #region pinvoke
        
        // kernel32.dll imports
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer, int size, IntPtr bytesReadBuffer);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualAllocEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int allocationType, int protection);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFreeEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int freeType);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtectEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int newProtection, out int oldProtection);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualQueryEx(SafeProcessHandle processHandle, IntPtr baseAddress, out MemoryBasicInformation buffer, int length);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer, int size, IntPtr bytesWrittenBuffer);
        
        #endregion
        
        #region Enumerations
        
        [Flags]
        internal enum MemoryAllocation
        {
            Commit = 0x01000,
            Reserve = 0x02000,
            Release = 0x08000
        }
        
        [Flags]
        internal enum MemoryProtection
        {
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x010,
            ExecuteRead = 0x020,
            ExecuteReadWrite = 0x040,
            ExecuteWriteCopy = 0x080,
            Guard = 0x0100,
            NoCache = 0x0200,
            WriteCombine = 0x0400
        }

        [Flags]
        internal enum MemoryRegionType
        {
            MemoryImage = 0x01000000
        }
        
        #endregion
        
        #region Structures
        
        [StructLayout(LayoutKind.Sequential)]
        internal struct MemoryBasicInformation
        {
            internal readonly IntPtr BaseAddress;
                
            private readonly IntPtr AllocationBase;
            private readonly uint AllocationProtect;
            
            internal readonly IntPtr RegionSize;
            
            internal readonly uint State;
            internal readonly uint Protect;
            internal readonly uint Type;
        }
        
        #endregion
    }
}