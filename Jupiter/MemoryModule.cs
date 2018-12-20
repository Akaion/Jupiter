using System;
using System.Runtime.InteropServices;
using Jupiter.Wrappers;

namespace Jupiter
{   
    public class MemoryModule
    {      
        #region AllocateMemory
        
        public IntPtr AllocateMemory(string processName, int size)
        {
            return new MethodWrapper(processName).AllocateMemory(size);
        }
        
        public IntPtr AllocateMemory(int processId, int size)
        {
            return new MethodWrapper(processId).AllocateMemory(size);
        }
        
        public IntPtr AllocateMemory(SafeHandle processHandle, int size)
        {
            return new MethodWrapper(processHandle).AllocateMemory(size);  
        }

        #endregion
        
        #region FreeMemory
        
        public bool FreeMemory(string processName, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processName).FreeMemory(baseAddress, size);
        }
        
        public bool FreeMemory(int processId, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processId).FreeMemory(baseAddress, size);
        }
        
        public bool FreeMemory(SafeHandle processHandle, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processHandle).FreeMemory(baseAddress, size);
        }
        
        #endregion
        
        #region PatternScan
        
        public IntPtr[] PatternScan(string processName, IntPtr baseAddress, string pattern)
        {
            return new ExtensionWrapper(processName).PatternScan(baseAddress, pattern);
        }
        
        public IntPtr[] PatternScan(int processId, IntPtr baseAddress, string pattern)
        {
            return new ExtensionWrapper(processId).PatternScan(baseAddress, pattern);
        }
        
        public IntPtr[] PatternScan(SafeHandle processHandle, IntPtr baseAddress, string pattern)
        {
            return new ExtensionWrapper(processHandle).PatternScan(baseAddress, pattern);
        }
        
        #endregion

        #region ProtectMemory
        
        public bool ProtectMemory(string processName, IntPtr baseAddress, int size, int protection)
        {
            return new MethodWrapper(processName).ProtectMemory(baseAddress, size, protection);
        }
        
        public bool ProtectMemory(int processId, IntPtr baseAddress, int size, int protection)
        {
            return new MethodWrapper(processId).ProtectMemory(baseAddress, size, protection);
        }
        
        public bool ProtectMemory(SafeHandle processHandle, IntPtr baseAddress, int size, int protection)
        {
            return new MethodWrapper(processHandle).ProtectMemory(baseAddress, size, protection);
        }
        
        #endregion
        
        #region ReadMemory
        
        public byte[] ReadMemory(string processName, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processName).ReadMemory(baseAddress, size);
        }
        
        public byte[] ReadMemory(int processId, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processId).ReadMemory(baseAddress, size);
        }
        
        public byte[] ReadMemory(SafeHandle processHandle, IntPtr baseAddress, int size)
        {
            return new MethodWrapper(processHandle).ReadMemory(baseAddress, size);
        }

        public TStructure ReadMemory<TStructure>(string processName, IntPtr baseAddress) where TStructure : struct
        {
            return new MethodWrapper(processName).ReadMemory<TStructure>(baseAddress);   
        }
        
        public TStructure ReadMemory<TStructure>(int processId, IntPtr baseAddress) where TStructure : struct
        {     
            return new MethodWrapper(processId).ReadMemory<TStructure>(baseAddress);  
        }
        
        public TStructure ReadMemory<TStructure>(SafeHandle processHandle, IntPtr baseAddress) where TStructure : struct
        {     
            return new MethodWrapper(processHandle).ReadMemory<TStructure>(baseAddress);    
        }
        
        #endregion

        #region WriteMemory
        
        public bool WriteMemory(string processName, IntPtr baseAddress, byte[] buffer)
        {
            return new MethodWrapper(processName).WriteMemory(baseAddress, buffer);
        }
        
        public bool WriteMemory(int processId, IntPtr baseAddress, byte[] buffer)
        {
            return new MethodWrapper(processId).WriteMemory(baseAddress, buffer);
        }
        
        public bool WriteMemory(SafeHandle processHandle, IntPtr baseAddress, byte[] buffer)
        {
            return new MethodWrapper(processHandle).WriteMemory(baseAddress, buffer);
        }

        public bool WriteMemory(string processName, IntPtr baseAddress, string s)
        {
            return new MethodWrapper(processName).WriteMemory(baseAddress, s);
        }
        
        public bool WriteMemory(int processId, IntPtr baseAddress, string s)
        {
            return new MethodWrapper(processId).WriteMemory(baseAddress, s);
        }
        
        public bool WriteMemory(SafeHandle processHandle, IntPtr baseAddress, string s)
        {
            return new MethodWrapper(processHandle).WriteMemory(baseAddress, s);
        }
        
        public bool WriteMemory<TStructure>(string processName, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            return new MethodWrapper(processName).WriteMemory(baseAddress, structure);
        }
        
        public bool WriteMemory<TStructure>(int processId, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            return new MethodWrapper(processId).WriteMemory(baseAddress, structure);
        }
        
        public bool WriteMemory<TStructure>(SafeHandle processHandle, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            return new MethodWrapper(processHandle).WriteMemory(baseAddress, structure);
        }
        
        #endregion
    }
}