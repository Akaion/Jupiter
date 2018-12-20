using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Jupiter.Methods;
using static Jupiter.Etc.Native;

namespace Jupiter.Extensions
{
    internal static class PatternScanner
    {
        internal static IntPtr[] Scan(SafeHandle processHandle, IntPtr baseAddress, string[] pattern)
        {
            // Initialize a list to store the memory regions of the process

            var memoryRegions = new ConcurrentBag<MemoryBasicInformation>
            {
                // Get the first memory region
                
                QueryMemory(processHandle, baseAddress)      
            };
        
            while (true)
            {
                // Get the address of the next memory region
                
                var nextMemoryRegionAddress = (long) memoryRegions.First().BaseAddress + (long) memoryRegions.First().RegionSize;

                // Get the next memory region
                
                var nextMemoryRegion = QueryMemory(processHandle, (IntPtr) nextMemoryRegionAddress);

                if (nextMemoryRegion.BaseAddress == IntPtr.Zero)
                {
                    // Last memory region
                    
                    break;
                }
                
                memoryRegions.Add(nextMemoryRegion);
            }
            
            // Filter the memory regions to avoid searching unnecessary regions
            
            var filteredMemoryRegions = memoryRegions.Where(memoryRegion => memoryRegion.State == (int) MemoryAllocation.Commit
                                                                         && memoryRegion.Protect != (int) MemoryProtection.PageNoAccess
                                                                         && memoryRegion.Protect != (int) MemoryProtection.PageGuard
                                                                         && memoryRegion.Type != (int) MemoryRegionType.MemoryImage);
            
            // Search the filtered memory regions for the pattern
            
            var patternAddresses = new ConcurrentBag<IntPtr>();
            
            Parallel.ForEach(filteredMemoryRegions, memoryRegion =>
            {
                var address = FindPattern(processHandle, memoryRegion, pattern);

                patternAddresses.Add(address);
            });

            // Return an array of addresses where the pattern was found
            
            return patternAddresses.Where(address => address != IntPtr.Zero).ToArray();
        }

        private static MemoryBasicInformation QueryMemory(SafeHandle processHandle, IntPtr baseAddress)
        {
            var memoryInformationSize = Marshal.SizeOf(typeof(MemoryBasicInformation));
            
            return VirtualQueryEx(processHandle, baseAddress, out var memoryInformation, memoryInformationSize) ? memoryInformation : default;
        }
        
        private static IntPtr FindPattern(SafeHandle processHandle, MemoryBasicInformation memoryRegion, IReadOnlyList<string> pattern)
        {
            // Ensure the memory region size is valid

            if ((long) memoryRegion.RegionSize > int.MaxValue)
            {
                return IntPtr.Zero;
            }
            
            // Get the bytes of the memory region
            
            var memoryRegionBytes = ReadMemory.Read(processHandle, memoryRegion.BaseAddress, (int) memoryRegion.RegionSize);

            if (memoryRegionBytes == null)
            {
                return IntPtr.Zero;
            }
            
            // Calculate the indexes of any wildcard bytes

            var wildCardIndexArray = pattern.Select((wildcard, index) => wildcard == "??" ? index : -1).Where(index => index != -1).ToArray();
            
            // Search the memory region for the pattern 
            
            var patternAddress = IntPtr.Zero;
            
            // Initialize a lookup directory
            
            var lookUpDirectory = new int[byte.MaxValue + 1];

            foreach (var index in Enumerable.Range(0, lookUpDirectory.Length))
            {
                lookUpDirectory[index] = pattern.Count;
            }

            // Initialize the pattern in the lookup directory
            
            foreach (var index in Enumerable.Range(0, pattern.Count))
            {
                if (wildCardIndexArray.Contains(index))
                {
                    break;
                }
                
                lookUpDirectory[int.Parse(pattern[index], NumberStyles.HexNumber)] = pattern.Count - index - 1;
            }

            var patternIndex = pattern.Count - 1;

            // Get the last byte in the pattern
            
            var lastByte = int.Parse(pattern.Last(), NumberStyles.HexNumber);

            // Search for the pattern in the memory region
            
            while (patternIndex < memoryRegionBytes.Length)
            {
                var currentByte = memoryRegionBytes[patternIndex];

                if (currentByte == lastByte)
                {
                    // Check if the pattern exists at the current index
                    
                    var patternFound = Enumerable.Range(0, pattern.Count - 2).Reverse().All(index => pattern[index] == "??" || memoryRegionBytes[patternIndex - pattern.Count + index + 1] == int.Parse(pattern[index], NumberStyles.HexNumber));

                    if (patternFound)
                    {
                        return memoryRegion.BaseAddress + (patternIndex - pattern.Count + 1);
                    }

                    patternIndex += 1;
                }

                else
                {
                    patternIndex += lookUpDirectory[currentByte];
                }
            }

            return patternAddress;
        }
    }
}