using System;
using System.Runtime.InteropServices;

namespace Jupiter.Native
{
    internal static class Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MemoryBasicInformation
        {
            internal readonly IntPtr BaseAddress;
                
            private readonly IntPtr AllocationBase;
            private readonly uint AllocationProtect;
            
            internal readonly IntPtr RegionSize;

            private readonly uint State;
            
            internal readonly MemoryProtection Protect;

            private readonly uint Type;
        }
    }
}