using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jupiter.Wrappers
{
    internal class ExtensionWrapper
    {
        private readonly SafeHandle _processHandle;
        
        internal ExtensionWrapper(string processName)
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

                throw new Exception($"No process with name {processName} is currently running");
            }
            
            // Get a handle to the process

            _processHandle = process.SafeHandle;
        }
        
        internal ExtensionWrapper(int processId)
        {
            // Ensure the argument passed in is valid

            if (processId == 0)
            {
                throw new ArgumentException("One or more of the arguments provided was invalid");
            }
            
            // Get an instance of the process
            
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
            
            // Get a handle to the process

            _processHandle = process.SafeHandle;
        }
        
        internal ExtensionWrapper(SafeHandle processHandle)
        {
            // Ensure the argument passed in is valid

            _processHandle = processHandle ?? throw new ArgumentException("One or more of the arguments provided was invalid");
        }
        
        internal IntPtr[] PatternScan(IntPtr baseAddress, string pattern)
        {
            var patternBytes = pattern.Split();
            
            // Ensure the pattern is valid

            if (patternBytes.Any(patternByte => patternByte != "??" && ! int.TryParse(patternByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)))
            {
                throw new ArgumentException("The pattern provided contained one or more invalid characters");
            }
            
            // Remove unnecessary wildcards

            patternBytes = patternBytes.SkipWhile(patternByte => patternByte.Equals("??"))
                                       .Reverse()
                                       .SkipWhile(patternByte => patternByte.Equals("??"))
                                       .Reverse().ToArray();

            return Extensions.PatternScanner.Scan(_processHandle, baseAddress, patternBytes);
        } 
    }
}