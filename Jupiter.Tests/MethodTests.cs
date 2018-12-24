using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Jupiter.Tests
{
    public class MethodTests
    {
        private readonly MemoryModule _memoryModule;

        private readonly string _hostProcessName;
                
        public MethodTests()
        {   
            _memoryModule = new MemoryModule();

            // Get the process id of the host process
            
            _hostProcessName = Process.GetCurrentProcess().ProcessName;
        }
        
        [Fact]
        public void TestAllocateMemory()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            Assert.True(testRegionAddress != IntPtr.Zero);
            
            // Free the memory that was allocated

            _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
        }
        
        [Fact]
        public void TestFreeMemory()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            // Free the memory that was allocated

            var result = _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
            
            Assert.True(result);
        }
        
        [Fact]
        public void TestProtectMemory()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            // Protect the memory that was allocated

            var result = _memoryModule.ProtectMemory(_hostProcessName, testRegionAddress, testRegionSize, 0x01);
            
            Assert.True(result);
            
            // Free the memory that was allocated

            _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
        }
        
        [Fact]
        public void TestReadMemory()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            // Write a test array of bytes into memory

            var testArray = new byte[] {0xFF, 0x01, 0x5A, 0xFF, 0x22, 0x00};

            _memoryModule.WriteMemory(_hostProcessName, testRegionAddress, testArray);
            
            // Read the test array of bytes from memory

            var readTestArray = _memoryModule.ReadMemory(_hostProcessName, testRegionAddress, testArray.Length);
            
            Assert.True(testArray.SequenceEqual(readTestArray));
            
            // Free the memory that was allocated

            _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
        }
        
        [Fact]
        public void TestWriteMemory()
        {
            // Allocate memory in the host process

            const int testRegionSize = 32;
            
            var testRegionAddress = _memoryModule.AllocateMemory(_hostProcessName, testRegionSize);
            
            // Write a test array of bytes into memory

            var testArray = new byte[] {0xFF, 0x01, 0x5A, 0xFF, 0x22, 0x00};

            var result = _memoryModule.WriteMemory(_hostProcessName, testRegionAddress, testArray);
            
            Assert.True(result);
            
            // Free the memory that was allocated

            _memoryModule.FreeMemory(_hostProcessName, testRegionAddress);
        }
    }
}