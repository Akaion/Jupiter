using System;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using static Jupiter.Etc.Native;

namespace Jupiter.Wrappers
{
    internal class MethodWrapper : IDisposable
    {
        private readonly SafeProcessHandle _processHandle;
        
        internal MethodWrapper(string processName)
        {
            // Ensure the argument passed in is valid

            if (string.IsNullOrWhiteSpace(processName))
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get an instance of the process
            
            Process process;

            try
            {
                process = Process.GetProcessesByName(processName)[0];
            }

            catch (IndexOutOfRangeException)
            {
                // The process isn't currently running

                throw new ArgumentException($"No process with name {processName} is currently running");
            }
            
            // Get a handle to the process

            _processHandle = process.SafeHandle;
        }
        
        internal MethodWrapper(int processId)
        {
            // Ensure the argument passed in is valid

            if (processId <= 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get an instance of the process
            
            Process process;

            try
            {
                process = Process.GetProcessById(processId);
            }

            catch (ArgumentException)
            {
                // The process isn't currently running

                throw new ArgumentException($"No process with id {processId} is currently running");
            }
            
            // Get a handle to the process

            _processHandle = process.SafeHandle;
        }
        
        public void Dispose()
        {
            _processHandle?.Close();
        }
        
        internal IntPtr AllocateMemory(int size)
        {
            // Ensure the argument passed in is valid

            if (size <= 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.AllocateMemory.Allocate(_processHandle, size);
        }

        internal bool FreeMemory(IntPtr baseAddress)
        {
            // Ensure the arguments passed in are valid

            if (baseAddress == IntPtr.Zero)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.FreeMemory.Free(_processHandle, baseAddress);
        }

        internal bool ProtectMemory(IntPtr baseAddress, int size, int protection)
        {
            // Ensure the arguments passed in are valid

            if (baseAddress == IntPtr.Zero || size <= 0 || !Enum.IsDefined(typeof(MemoryProtection), protection))
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }

            return Methods.ProtectMemory.Protect(_processHandle, baseAddress, size, protection);
        }
        
        internal byte[] ReadMemory(IntPtr baseAddress, int size)
        {
            // Ensure the arguments passed in are valid

            if (baseAddress == IntPtr.Zero || size <= 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.ReadMemory.Read(_processHandle, baseAddress, size);
        }
        
        internal TStructure ReadMemory<TStructure>(IntPtr baseAddress) where TStructure : struct
        {
            // Ensure the argument passed in is valid

            if (baseAddress == IntPtr.Zero)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.ReadMemory.Read<TStructure>(_processHandle, baseAddress);
        }
        
        internal bool WriteMemory(IntPtr baseAddress, byte[] buffer)
        {
            // Ensure the arguments passed in are valid

            if (baseAddress == IntPtr.Zero || buffer is null)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.WriteMemory.Write(_processHandle, baseAddress, buffer);
        }

        internal bool WriteMemory(IntPtr baseAddress, string s)
        {
            // Ensure the arguments passed in are valid

            if (baseAddress == IntPtr.Zero || string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }

            return Methods.WriteMemory.Write(_processHandle, baseAddress, s);
        }
        
        internal bool WriteMemory<TStructure>(IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            // Ensure the argument passed in is valid

            if (baseAddress == IntPtr.Zero)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            return Methods.WriteMemory.Write(_processHandle, baseAddress, structure);
        }
    }
}