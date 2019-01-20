using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Win32.SafeHandles;

namespace Jupiter.Wrappers
{
    internal class ExtensionWrapper : IDisposable
    {
        private readonly SafeProcessHandle _processHandle;
        
        internal ExtensionWrapper(string processName)
        {
            // Ensure the argument passed in is valid

            if (string.IsNullOrWhiteSpace(processName))
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get an instance of the remote process
            
            Process process;

            try
            {
                process = Process.GetProcessesByName(processName)[0];
            }

            catch (IndexOutOfRangeException)
            {
                // The process isn't currently running

                throw new Exception($"No process with name {processName} is currently running");
            }
            
            // Get a handle to the remote process

            _processHandle = process.SafeHandle;
        }
        
        internal ExtensionWrapper(int processId)
        {
            // Ensure the argument passed in is valid

            if (processId <= 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get an instance of the remote process
            
            Process process;

            try
            {
                process = Process.GetProcessById(processId);
            }

            catch (IndexOutOfRangeException)
            {
                // The process isn't currently running

                throw new Exception($"No process with id {processId} is currently running");
            }
            
            // Get a handle to the remote process

            _processHandle = process.SafeHandle;
        }
        
        public void Dispose()
        {
            _processHandle?.Close();
        }
        
        internal IntPtr[] PatternScan(IntPtr baseAddress, string pattern)
        {
            // Ensure the arguments passed in are valid

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            var patternByteList = pattern.Split().ToList();
            
            // Ensure the pattern is valid

            if (patternByteList.Any(patternByte => patternByte != "??" && ! int.TryParse(patternByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)))
            {
                throw new ArgumentException("The pattern provided contained one or more invalid characters");
            }
            
            // Remove unnecessary wildcards

            patternByteList = patternByteList.SkipWhile(patternByte => patternByte.Equals("??"))
                                             .Reverse()
                                             .SkipWhile(patternByte => patternByte.Equals("??"))
                                             .Reverse().ToList();
            
            return Extensions.PatternScanner.Scan(_processHandle, baseAddress, patternByteList);
        }
        
        internal IntPtr[] PatternScan(IntPtr baseAddress, byte[] patternBytes)
        {
            // Ensure the arguments passed in are valid

            if (patternBytes is null || patternBytes.Length == 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get the hexadecimal representation of each byte
            
            var pattern = BitConverter.ToString(patternBytes).Replace("-", " ");
            
            var patternByteList = pattern.Split().ToList();
            
            // Ensure the pattern is valid

            if (patternByteList.Any(patternByte => patternByte != "??" && ! int.TryParse(patternByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)))
            {
                throw new ArgumentException("The pattern provided contained one or more invalid characters");
            }
            
            // Remove unnecessary wildcards

            patternByteList = patternByteList.SkipWhile(patternByte => patternByte.Equals("??"))
                                             .Reverse()
                                             .SkipWhile(patternByte => patternByte.Equals("??"))
                                             .Reverse().ToList();
            
            return Extensions.PatternScanner.Scan(_processHandle, baseAddress, patternByteList);
        }
    }
}