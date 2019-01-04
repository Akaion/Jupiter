using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class ReadMemory
    {
        internal static byte[] Read(SafeProcessHandle processHandle, IntPtr baseAddress, int size)
        {
            var buffer = new byte[size];

            // Change the protection of the memory region at the address
            
            if (!VirtualProtectEx(processHandle, baseAddress, buffer.Length, (int) MemoryProtection.PageReadWrite, out var oldProtection))
            {
                return null;
            }
            
            // Read the memory from the memory region into the buffer

            if (!ReadProcessMemory(processHandle, baseAddress, buffer, buffer.Length, 0))
            {
                return null;
            }
            
            // Restore the protection of the memory region at the address
            
            if (!VirtualProtectEx(processHandle, baseAddress, buffer.Length, oldProtection, out _))
            {
                return null;
            }
            
            return buffer;
        }

        internal static TStructure Read<TStructure>(SafeProcessHandle processHandle, IntPtr baseAddress) where TStructure : struct
        {   
            // Get the size of the structure
            
            var size = Marshal.SizeOf(typeof(TStructure));
            
            // Read the memory from the memory region

            var buffer = Read(processHandle, baseAddress, size);

            if (buffer is null)
            {
                return default;
            }
            
            // Allocate temporary memory to store the buffer

            var bufferAddress = Marshal.AllocHGlobal(buffer.Length);
            
            // Copy the buffer into the temporary memory region
            
            Marshal.Copy(buffer, 0, bufferAddress, buffer.Length);
            
            // Convert the buffer into a structure
            
            var structure = (TStructure) Marshal.PtrToStructure(bufferAddress, typeof(TStructure));
            
            // Free the temporary allocated memory
            
            Marshal.FreeHGlobal(bufferAddress);
            
            return structure;
        }
    }
}