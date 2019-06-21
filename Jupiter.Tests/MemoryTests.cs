using System;
using System.ComponentModel;
using System.Diagnostics;
using Xunit;

namespace Jupiter.Tests
{
    public class MemoryTests
    {
        private readonly MemoryModule _memoryModule;

        public MemoryTests()
        {
            _memoryModule = new MemoryModule(Process.GetCurrentProcess().Id);
        }

        [Fact]
        public void TestAllocateMemory()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int));
            
            Assert.True(regionAddress != IntPtr.Zero);
            
            _memoryModule.FreeVirtualMemory(regionAddress);
        }
        
        [Fact]
        public void TestFreeMemory()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int));
            
            _memoryModule.FreeVirtualMemory(regionAddress);

            Assert.Throws<Win32Exception>(() => _memoryModule.ReadVirtualMemory<int>(regionAddress));
        }

        [Fact]
        public void TestPatternScan()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int));

            var pattern = new byte[] {0x6A, 0x8B, 0xFF, 0xFF, 0x11, 0x60, 0x00, 0x1A};
            
            _memoryModule.WriteVirtualMemory(regionAddress, pattern);
            
            Assert.Contains(regionAddress, _memoryModule.PatternScan(pattern));
            
            _memoryModule.FreeVirtualMemory(regionAddress);
        }
        
        [Fact]
        public void TestProtectMemory()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int), MemoryProtection.ReadWrite);

            _memoryModule.ProtectVirtualMemory(regionAddress, sizeof(int), MemoryProtection.NoAccess);
            
            Assert.Throws<Win32Exception>(() => _memoryModule.ReadVirtualMemory<int>(regionAddress));
            
            _memoryModule.FreeVirtualMemory(regionAddress);
        }
        
        [Fact]
        public void TestReadMemory()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int));
            
            _memoryModule.WriteVirtualMemory(regionAddress, 15);

            Assert.Equal(15, _memoryModule.ReadVirtualMemory<int>(regionAddress));
            
            _memoryModule.FreeVirtualMemory(regionAddress);
        }
        
        [Fact]
        public void TestWriteMemory()
        {
            var regionAddress = _memoryModule.AllocateVirtualMemory(sizeof(int));
            
            _memoryModule.WriteVirtualMemory(regionAddress, 25);
            
            Assert.Equal(25, _memoryModule.ReadVirtualMemory<int>(regionAddress));
            
            _memoryModule.FreeVirtualMemory(regionAddress);
        }
    }
}