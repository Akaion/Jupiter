using System;
using Jupiter.Wrappers;

namespace Jupiter
{   
    public class MemoryModule
    {      
        #region AllocateMemory
        
        public IntPtr AllocateMemory(string processName, int size)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.AllocateMemory(size);
            }
        }
        
        public IntPtr AllocateMemory(int processId, int size)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.AllocateMemory(size);
            }
        }

        #endregion
        
        #region FreeMemory
        
        public bool FreeMemory(string processName, IntPtr baseAddress)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.FreeMemory(baseAddress);
            }
        }
        
        public bool FreeMemory(int processId, IntPtr baseAddress)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.FreeMemory(baseAddress);
            }
        }
        
        #endregion
        
        #region PatternScan
        
        public IntPtr[] PatternScan(string processName, IntPtr baseAddress, string pattern)
        {
            using (var extensionWrapper = new ExtensionWrapper(processName))
            {
                return extensionWrapper.PatternScan(baseAddress, pattern);
            }
        }
        
        public IntPtr[] PatternScan(int processId, IntPtr baseAddress, string pattern)
        {
            using (var extensionWrapper = new ExtensionWrapper(processId))
            {
                return extensionWrapper.PatternScan(baseAddress, pattern);
            }
        }
        
        public IntPtr[] PatternScan(string processName, IntPtr baseAddress, byte[] patternBytes)
        {
            using (var extensionWrapper = new ExtensionWrapper(processName))
            {
                return extensionWrapper.PatternScan(baseAddress, patternBytes);
            }
        }
        
        public IntPtr[] PatternScan(int processId, IntPtr baseAddress, byte[] patternBytes)
        {
            using (var extensionWrapper = new ExtensionWrapper(processId))
            {
                return extensionWrapper.PatternScan(baseAddress, patternBytes);
            }
        }
        
        #endregion

        #region ProtectMemory
        
        public bool ProtectMemory(string processName, IntPtr baseAddress, int size, int protection)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.ProtectMemory(baseAddress, size, protection);
            }
        }
        
        public bool ProtectMemory(int processId, IntPtr baseAddress, int size, int protection)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.ProtectMemory(baseAddress, size, protection);
            }
        }
        
        #endregion
        
        #region ReadMemory
        
        public byte[] ReadMemory(string processName, IntPtr baseAddress, int size)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.ReadMemory(baseAddress, size);
            }
        }
        
        public byte[] ReadMemory(int processId, IntPtr baseAddress, int size)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.ReadMemory(baseAddress, size);
            }
        }

        public TStructure ReadMemory<TStructure>(string processName, IntPtr baseAddress) where TStructure : struct
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.ReadMemory<TStructure>(baseAddress);
            }  
        }
        
        public TStructure ReadMemory<TStructure>(int processId, IntPtr baseAddress) where TStructure : struct
        {    
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.ReadMemory<TStructure>(baseAddress);
            }  
        }
        
        #endregion

        #region WriteMemory
        
        public bool WriteMemory(string processName, IntPtr baseAddress, byte[] buffer)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.WriteMemory(baseAddress, buffer);
            }
        }
        
        public bool WriteMemory(int processId, IntPtr baseAddress, byte[] buffer)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.WriteMemory(baseAddress, buffer);
            }
        }
        
        public bool WriteMemory(string processName, IntPtr baseAddress, string s)
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.WriteMemory(baseAddress, s);
            }
        }
        
        public bool WriteMemory(int processId, IntPtr baseAddress, string s)
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.WriteMemory(baseAddress, s);
            }
        }
        
        public bool WriteMemory<TStructure>(string processName, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            using (var methodWrapper = new MethodWrapper(processName))
            {
                return methodWrapper.WriteMemory(baseAddress, structure);
            }
        }
        
        public bool WriteMemory<TStructure>(int processId, IntPtr baseAddress, TStructure structure) where TStructure : struct
        {
            using (var methodWrapper = new MethodWrapper(processId))
            {
                return methodWrapper.WriteMemory(baseAddress, structure);
            }
        }
        
        #endregion
    }
}