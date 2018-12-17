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
        internal static IntPtr Scan(SafeHandle processHandle, IntPtr baseAddress, string[] pattern)
        {
            // Initialize a list to store the memory regions of the process

            var memoryRegions = new ConcurrentBag<MemoryBasicInformation>
            {
                // Get the first memory region address
                
                QueryMemory(processHandle, baseAddress)      
            };
        
            while (true)
            {
                // Get the next memory region address
                
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

            var filterByState = memoryRegions.Where(memoryRegion => memoryRegion.State == (int) MemoryAllocation.Commit);
            
            var filterByProtection = filterByState.Where(memoryRegion => memoryRegion.Protect != (int) MemoryProtection.PageNoAccess && memoryRegion.Protect != (int) MemoryProtection.PageGuard);

            var filterByType = filterByProtection.Where(memoryRegion => memoryRegion.Type != (int) MemoryRegionType.MemoryImage);
            
            // Search the filtered memory regions for the pattern
            
            var patternAddresses = new ConcurrentBag<IntPtr>();
            
            Parallel.ForEach(filterByType, memoryRegion =>
            {
                var address = FindPattern(processHandle, memoryRegion, pattern);

                patternAddresses.Add(address);
            });

            // Return the first pattern address
            
            return patternAddresses.FirstOrDefault(address => address != IntPtr.Zero);
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
            
            // Search the memory region for the pattern 
            
            var patternAddress = IntPtr.Zero;
            
            foreach (var regionByteIndex in Enumerable.Range(0, memoryRegionBytes.Length))
            {
                // If the byte matches the first byte of the pattern
                
                if (memoryRegionBytes[regionByteIndex] == int.Parse(pattern[0], NumberStyles.HexNumber))
                {
                    // Get an array of bytes from the memory region equal to the length of the pattern
                    
                    var tempArray = memoryRegionBytes.Skip(regionByteIndex).Take(pattern.Count).ToArray();

                    // Compare the array to the pattern
                    
                    var counter = Enumerable.Range(0, tempArray.Length).Count(tempByteIndex => pattern[tempByteIndex] == "??" || tempArray[tempByteIndex] == int.Parse(pattern[tempByteIndex], NumberStyles.HexNumber));

                    // If the array matches the pattern
                    
                    if (counter == pattern.Count)
                    {
                        // Get the address of the pattern
                        
                        patternAddress = memoryRegion.BaseAddress + regionByteIndex;

                        break;
                    }
                }
            }

            return patternAddress;
        }
    }
}