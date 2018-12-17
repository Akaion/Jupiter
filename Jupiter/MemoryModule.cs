using System;
using System.Runtime.InteropServices;
using Jupiter.Wrapper;
namespace Jupiter
{
    public class MemoryModule
    {
        public IntPtr AllocateMemory(string processName, int size)
        {
            return new MemoryWrapper(processName).AllocateMemory(size);
        }
        
        public IntPtr AllocateMemory(int processId, int size)
        {
            return new MemoryWrapper(processId).AllocateMemory(size);
        }
        
        public IntPtr AllocateMemory(SafeHandle processHandle, int size)
        {
            return new MemoryWrapper(processHandle).AllocateMemory(size);
        }

        public bool FreeMemory(string processName, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processName).FreeMemory(baseAddress, size);
        }
        
        public bool FreeMemory(int processId, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processId).FreeMemory(baseAddress, size);
        }
        
        public bool FreeMemory(SafeHandle processHandle, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processHandle).FreeMemory(baseAddress, size);
        }
        
        public IntPtr PatternScan(string processName, IntPtr baseAddress, string pattern)
        {   
            return new MemoryWrapper(processName).PatternScan(baseAddress, pattern);
        }
        
        public IntPtr PatternScan(int processId, IntPtr baseAddress, string pattern)
        {
            return new MemoryWrapper(processId).PatternScan(baseAddress, pattern);
        }

        public IntPtr PatternScan(SafeHandle processHandle, IntPtr baseAddress, string pattern)
        {
            return new MemoryWrapper(processHandle).PatternScan(baseAddress, pattern);
        }

        public byte[] ReadMemory(string processName, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processName).ReadMemory(baseAddress, size);
        }
        
        public byte[] ReadMemory(int processId, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processId).ReadMemory(baseAddress, size);
        }
        
        public byte[] ReadMemory(SafeHandle processHandle, IntPtr baseAddress, int size)
        {
            return new MemoryWrapper(processHandle).ReadMemory(baseAddress, size);
        }

        public TStructure ReadMemory<TStructure>(string processName, IntPtr baseAddress)
        {
            return new MemoryWrapper(processName).ReadMemory<TStructure>(baseAddress);   
        }
        
        public TStructure ReadMemory<TStructure>(int processId, IntPtr baseAddress)
        {
            return new MemoryWrapper(processId).ReadMemory<TStructure>(baseAddress);   
        }
        
        public TStructure ReadMemory<TStructure>(SafeHandle processHandle, IntPtr baseAddress)
        {
            return new MemoryWrapper(processHandle).ReadMemory<TStructure>(baseAddress);   
        }

        public bool WriteMemory(string processName, IntPtr baseAddress, byte[] buffer)
        {
            return new MemoryWrapper(processName).WriteMemory(baseAddress, buffer);
        }
        
        public bool WriteMemory(int processId, IntPtr baseAddress, byte[] buffer)
        {
            return new MemoryWrapper(processId).WriteMemory(baseAddress, buffer);
        }
        
        public bool WriteMemory(SafeHandle processHandle, IntPtr baseAddress, byte[] buffer)
        {
            return new MemoryWrapper(processHandle).WriteMemory(baseAddress, buffer);
        }

        public bool WriteMemory<TStructure>(string processName, IntPtr baseAddress, TStructure structure)
        {
            return new MemoryWrapper(processName).WriteMemory(baseAddress, structure);
        }
        
        public bool WriteMemory<TStructure>(int processId, IntPtr baseAddress, TStructure structure)
        {
            return new MemoryWrapper(processId).WriteMemory(baseAddress, structure);
        }
        
        public bool WriteMemory<TStructure>(SafeHandle processHandle, IntPtr baseAddress, TStructure structure)
        {
            return new MemoryWrapper(processHandle).WriteMemory(baseAddress, structure);
        }
    }
}