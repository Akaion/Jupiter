using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Jupiter.Memory;
using Jupiter.Shared.Handlers;

namespace Jupiter
{
    /// <summary>
    /// Initialises an instance capable of managing memory in a specified remote process
    /// </summary>
    public class MemoryModule : IDisposable
    {
        private readonly MemoryManager _memoryManager;

        private readonly PatternScanner _patternScanner;
        
        /// <summary>
        /// Initialises an instance capable of managing memory in a specified remote process
        /// </summary>
        public MemoryModule(int processId)
        {
            ValidationHandler.ValidateOperatingSystem();
            
            _memoryManager = new MemoryManager(processId);
            
            _patternScanner = new PatternScanner(_memoryManager);
        }
        
        /// <summary>
        /// Initialises an instance capable of managing memory in a specified remote process
        /// </summary>
        public MemoryModule(string processName)
        {
            ValidationHandler.ValidateOperatingSystem();
            
            _memoryManager = new MemoryManager(processName);
            
            _patternScanner = new PatternScanner(_memoryManager);
        }
        
        /// <summary>
        /// Frees the unmanaged resources used by the instance
        /// </summary>
        public void Dispose()
        {
            _memoryManager.Dispose();
        }

        /// <summary>
        /// Allocates a region of virtual memory in the remote process
        /// </summary>
        public IntPtr AllocateVirtualMemory(IntPtr baseAddress, int allocationSize, MemoryProtection protectionType = MemoryProtection.ExecuteReadWrite)
        {
            return _memoryManager.AllocateVirtualMemory(baseAddress, allocationSize, protectionType);
        }
        
        /// <summary>
        /// Allocates a region of virtual memory in the remote process
        /// </summary>
        public IntPtr AllocateVirtualMemory(int allocationSize, MemoryProtection protectionType = MemoryProtection.ExecuteReadWrite)
        {
            return _memoryManager.AllocateVirtualMemory(IntPtr.Zero, allocationSize, protectionType);
        }

        /// <summary>
        /// Frees a region of virtual memory in the remote process
        /// </summary>
        public void FreeVirtualMemory(IntPtr baseAddress)
        {
            _memoryManager.FreeVirtualMemory(baseAddress);
        }

        /// <summary>
        /// Searches the memory of the remote process for the specified pattern
        /// </summary>
        public IEnumerable<IntPtr> PatternScan(IntPtr baseAddress, byte[] pattern)
        {
            // Ensure the arguments passed in are valid

            if (pattern is null || pattern.Length == 0)
            {
                throw new ArgumentException("One or more of the arguments provided were invalid");
            }
            
            // Convert the pattern into a hexadecimal string
            
            var hexPattern = BitConverter.ToString(pattern).Replace("-", " ").Split().ToList();

            return _patternScanner.FindPattern(baseAddress, hexPattern);
        }
        
        /// <summary>
        /// Searches the memory of the remote process for the specified pattern
        /// </summary>
        public IEnumerable<IntPtr> PatternScan(byte[] pattern)
        {
            return PatternScan(IntPtr.Zero, pattern);
        }
        
        /// <summary>
        /// Searches the memory of the remote process for the specified pattern
        /// </summary>
        public IEnumerable<IntPtr> PatternScan(IntPtr baseAddress, string pattern)
        {
            // Ensure the arguments passed in are valid

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("One or more of the arguments provided were invalid");
            }

            var patternBytes = pattern.Split().ToList();
            
            // Ensure the pattern is valid

            if (patternBytes.Any(patternByte => patternByte != "??" && !int.TryParse(patternByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)))
            {
                throw new ArgumentException("The pattern contained one or more invalid characters");
            }
            
            // Remove any unnecessary wildcards

            patternBytes = patternBytes.SkipWhile(patternByte => patternByte == "??")
                                       .Reverse()
                                       .SkipWhile(patternByte => patternByte == "??")
                                       .Reverse().ToList();

            return _patternScanner.FindPattern(baseAddress, patternBytes);
        }
        
        /// <summary>
        /// Searches the memory of the remote process for the specified pattern
        /// </summary>
        public IEnumerable<IntPtr> PatternScan(string pattern)
        {
            return PatternScan(IntPtr.Zero, pattern);
        }

        /// <summary>
        /// Changes the protection of a region of virtual memory in the remote process
        /// </summary>
        public MemoryProtection ProtectVirtualMemory(IntPtr baseAddress, int protectionSize, MemoryProtection protectionType)
        {
            return _memoryManager.ProtectVirtualMemory(baseAddress, protectionSize, protectionType);
        }
        
        /// <summary>
        /// Reads an array of bytes from a region of virtual memory in the remote process
        /// </summary>
        public byte[] ReadVirtualMemory(IntPtr baseAddress, int bytesToRead)
        {
            return _memoryManager.ReadVirtualMemory(baseAddress, bytesToRead);
        }
        
        /// <summary>
        /// Reads a structure from a region of virtual memory in the remote process
        /// </summary>
        public TStructure ReadVirtualMemory<TStructure>(IntPtr baseAddress) where TStructure : struct
        {
            return _memoryManager.ReadVirtualMemory<TStructure>(baseAddress);
        }
        
        /// <summary>
        /// Writes an array of bytes into a region of virtual memory in the remote process
        /// </summary>
        public void WriteVirtualMemory(IntPtr baseAddress, byte[] bytesToWrite)
        {
            _memoryManager.WriteVirtualMemory(baseAddress, bytesToWrite);
        }
        
        /// <summary>
        /// Writes a structure into a region of virtual memory in the remote process
        /// </summary>
        public void WriteVirtualMemory<TStructure>(IntPtr baseAddress, TStructure structureToWrite) where TStructure : struct
        {
            _memoryManager.WriteVirtualMemory(baseAddress, structureToWrite);
        }
    }
}