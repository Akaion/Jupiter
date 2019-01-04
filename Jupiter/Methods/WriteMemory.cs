using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
   internal static class WriteMemory
    {
        internal static bool Write(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer)
        {
            // Change the protection of the memory region at the address
            
            if (!VirtualProtectEx(processHandle, baseAddress, buffer.Length, (int) MemoryProtection.PageReadWrite, out var oldProtection))
            {
                return false;
            }
            
            // Write the buffer into the memory region
            
            if (!WriteProcessMemory(processHandle, baseAddress, buffer, buffer.Length, 0))
            {
                return false;
            }
            
            // Restore the protection of the memory region at the address
            
            if (!VirtualProtectEx(processHandle, baseAddress, buffer.Length, oldProtection, out _))
            {
                return false;
            }
            
            return true;
        }

        internal static bool Write(SafeProcessHandle processHandle, IntPtr baseAddress, string s)
        {
            // Get the byte representation of the string
                
            var bytes = Encoding.UTF8.GetBytes(s);
            
            // Write the string into the memory region at the address

            return Write(processHandle, baseAddress, bytes);
        }
        
        internal static bool Write<TStructure>(SafeProcessHandle processHandle, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            // Get the size of the structure
            
            var size = Marshal.SizeOf(typeof(TStructure));
            
            // Initialize a buffer to store the bytes of the structure

            var buffer = new byte[size];
            
            // Allocate temporary memory to store the structure
            
            var structureAddress = Marshal.AllocHGlobal(size);
            
            // Store the structure in the temporary memory region
            
            Marshal.StructureToPtr(structure, structureAddress, true);
            
            // Copy the structure from the temporary memory region into the buffer
            
            Marshal.Copy(structureAddress, buffer, 0, size);
            
            // Free the temporary allocated memory
            
            Marshal.FreeHGlobal(structureAddress);
            
            // Write the structure into the memory region at the address

            return Write(processHandle, baseAddress, buffer);
        }
    }
}