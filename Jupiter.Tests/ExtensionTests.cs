using System;
using System.Diagnostics;
using Xunit;

namespace Jupiter.Tests
{
    public class ExtensionTests
    {
        private readonly MemoryModule _memoryModule;

        private readonly string _hostProcessName;
                
        public ExtensionTests()
        {   
            _memoryModule = new MemoryModule();

            // Get the process id of the host process
            
            _hostProcessName = Process.GetCurrentProcess().ProcessName;
        }
        
        [Fact]
        public void TestPatternScan()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            // Write a test array of bytes into memory

            var testArray = new byte[] {0xFF, 0x01, 0x5A, 0xFF, 0x22, 0x00, 0x1A, 0xAA, 0xFF, 0xFF, 0xFF};

            _memoryModule.WriteMemory(_hostProcessName, testRegionAddress, testArray);
            
            // Scan the host process for test array pattern

            const string testArrayPattern = "FF 01 ?? FF 22 00 ?? AA FF ?? FF";

            var result = _memoryModule.PatternScan(_hostProcessName, IntPtr.Zero, testArrayPattern);
            
            Assert.True(testRegionAddress == result[0]);
            
            // Free the memory that was allocated

            _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
        }
    }
}