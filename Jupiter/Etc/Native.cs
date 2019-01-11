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
        internal static extern bool ReadProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer, int size, int bytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualAllocEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int allocationType, int protection);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFreeEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int freeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtectEx(SafeProcessHandle processHandle, IntPtr baseAddress, int size, int newProtection, out int oldProtection);
 
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualQueryEx(SafeProcessHandle processHandle, IntPtr baseAddress, out MemoryBasicInformation buffer, int length);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer, int size, int bytesWritten);
        
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
            PageNoAccess = 0x01,
            PageReadOnly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecute = 0x010,
            PageExecuteRead = 0x020,
            PageExecuteReadWrite = 0x040,
            PageExecuteWriteCopy = 0x080,
            PageGuard = 0x0100,
            PageNoCache = 0x0200,
            PageWriteCombine = 0x0400
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
            private readonly int AllocationProtect;

            internal readonly IntPtr RegionSize;

            internal readonly int State;
            internal readonly int Protect;
            internal readonly int Type;
        }
        
        #endregion
    }
}