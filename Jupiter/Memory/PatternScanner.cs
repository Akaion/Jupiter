using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Jupiter.Shared;
using static Jupiter.Native.Structures;

namespace Jupiter.Memory
{
    internal class PatternScanner
    {
        private readonly MemoryManager _memoryManager;

        internal PatternScanner(MemoryManager memoryManager)
        {
            _memoryManager = memoryManager;
        }

        internal List<IntPtr> FindPattern(IntPtr baseAddress, List<string> pattern)
        {
            var patternAddresses = new List<IntPtr>();

            GetMemoryRegions(baseAddress).AsParallel().ForAll(memoryRegion =>
            {
                // Read the bytes of the memory region

                var memoryRegionBytes = _memoryManager.ReadVirtualMemory(memoryRegion.BaseAddress, (int) memoryRegion.RegionSize);
                
                // Search the memory region for the pattern
                
                patternAddresses.AddRange(SearchMemoryRegion(memoryRegionBytes, pattern).TakeWhile(offset => offset != -1).Select(offset => memoryRegion.BaseAddress + offset));
            });

            return patternAddresses;
        }
        
        private int[] GenerateLookupTable(List<string> pattern)
        {
            var wildcardIndexes = pattern.Select((wildcard, index) => wildcard == "??" ? index : -1).Where(index => index != -1).ToList();
            
            // Generate a lookup table

            var lookupTable = new int[byte.MaxValue + 1];
            
            for (var tableIndex = 0; tableIndex < lookupTable.Length; tableIndex += 1)
            {
                lookupTable[tableIndex] = pattern.Count;
            }
            
            // Initialise the pattern in the lookup table

            for (var patternIndex = 0; patternIndex < pattern.Count - 1; patternIndex += 1)
            {
                if (!wildcardIndexes.Contains(patternIndex))
                {
                    lookupTable[int.Parse(pattern[patternIndex], NumberStyles.HexNumber)] = pattern.Count - patternIndex - 1;
                }
            }

            return lookupTable;
        }

        private IEnumerable<MemoryBasicInformation> GetMemoryRegions(IntPtr baseAddress)
        {
            var memoryRegionFilter = new List<MemoryProtection>
            {
                MemoryProtection.ZeroAccess,
                MemoryProtection.NoAccess,
                MemoryProtection.Guard,
                MemoryProtection.ReadWriteGuard
            };
            
            // Get the first memory region
            
            var memoryRegion = _memoryManager.QueryVirtualMemory(baseAddress);

            if (!memoryRegionFilter.Contains(memoryRegion.Protect))
            {
                yield return memoryRegion;
            }
            
            while (true)
            {
                // Get the next memory region

                try
                {
                    memoryRegion = _memoryManager.QueryVirtualMemory(memoryRegion.BaseAddress.AddOffset((ulong) memoryRegion.RegionSize));
                }

                catch (Win32Exception)
                {
                    break;
                }

                if (!memoryRegionFilter.Contains(memoryRegion.Protect))
                {
                    yield return memoryRegion;
                }
            }
        }

        private IEnumerable<int> SearchMemoryRegion(byte[] memoryRegionBytes, List<string> pattern)
        {
            var lookupTable = GenerateLookupTable(pattern);
            
            var offset = 0;
            
            // Search the memory region for the pattern

            while (memoryRegionBytes.Length - offset >= pattern.Count)
            {
                var lastByte = pattern.Count - 1;

                // Look for the pattern at the current offset

                while (pattern[lastByte] == "??" || memoryRegionBytes[offset + lastByte] == int.Parse(pattern[lastByte], NumberStyles.HexNumber))
                {
                    if (lastByte == 0)
                    {
                        yield return offset;

                        break;
                    }
                    
                    lastByte -= 1;
                }
                
                offset += lookupTable[memoryRegionBytes[offset + pattern.Count - 1]];
            }
            
            yield return -1;
        }
    }
}