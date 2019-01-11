using System;
using System.Runtime.InteropServices;
using System.Text;
using Jupiter.Etc;
using Jupiter.Services;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Methods
{
   internal static class WriteMemory
    {
        internal static bool Write(SafeProcessHandle processHandle, IntPtr baseAddress, byte[] buffer)
        {
            // Change the protection of the memory region at the address
            
            if (!Native.VirtualProtectEx(processHandle, baseAddress, buffer.Length, (int) Native.MemoryProtection.PageReadWrite, out var oldProtection))
            {
                ExceptionHandler.ThrowWin32Exception("Failed to protect memory in the process");
            }
            
            // Write the buffer into the memory region
            
            if (!Native.WriteProcessMemory(processHandle, baseAddress, buffer, buffer.Length, 0))
            {
                ExceptionHandler.ThrowWin32Exception("Failed to write memory in the process");
            }
            
            // Restore the protection of the memory region at the address
            
            if (!Native.VirtualProtectEx(processHandle, baseAddress, buffer.Length, oldProtection, out _))
            {
                ExceptionHandler.ThrowWin32Exception("Failed to protect memory in the process");
            }
            
            return true;
        }

        internal static bool Write(SafeProcessHandle processHandle, IntPtr baseAddress, string s)
        {
            // Get the byte representation of the string
                
            var stringBytes = Encoding.UTF8.GetBytes(s);
            
            // Write the string into the memory region at the address

            return Write(processHandle, baseAddress, stringBytes);
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