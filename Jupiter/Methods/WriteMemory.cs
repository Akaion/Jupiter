using System;
using System.Runtime.InteropServices;
using System.Text;
using static Jupiter.Etc.Native;

namespace Jupiter.Methods
{
    internal static class WriteMemory
    {
        internal static bool Write(SafeHandle processHandle, IntPtr baseAddress, byte[] buffer)
        {
            // Change the protection of the memory region at the address
            
            if (!VirtualProtectEx(processHandle, baseAddress, buffer.Length, MemoryProtection.PageReadWrite, out var oldProtection))
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

        internal static bool Write<TStructure>(SafeHandle processHandle, IntPtr baseAddress, TStructure structure)
        {            
            if (typeof(TStructure) == typeof(string))
            {
                // Get the byte representation of the string
                
                var bytes = Encoding.UTF8.GetBytes((string) (object) structure);
                
                // Write the string into the memory region at the address

                return Write(processHandle, baseAddress, bytes);
            }
            
            // Ensure the structure isn't a reference type
            
            if (!typeof(TStructure).IsValueType)
            {   
                throw new ArgumentException("The structure provided was a reference type with no predefined size");
            }

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