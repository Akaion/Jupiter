using System;

namespace Jupiter
{
    /// <summary>
    /// Defines the protection to be applied to a region of virtual memory
    /// </summary>
    [Flags]
    public enum MemoryProtection
    {
        /// <summary>
        /// Disables all access to the region of virtual memory
        /// </summary>
        ZeroAccess = 0x00,
        /// <summary>
        /// Disables all access to the region of virtual memory
        /// </summary>
        NoAccess = 0x01,
        /// <summary>
        /// Marks the region of virtual memory as readable
        /// </summary>
        ReadOnly = 0x02,
        /// <summary>
        /// Marks the region of virtual memory as readable and/or writable
        /// </summary>
        ReadWrite = 0x04,
        /// <summary>
        /// Marks the region of virtual memory as readable and/or copy on write
        /// </summary>
        WriteCopy = 0x08,
        /// <summary>
        /// Marks the region of virtual memory as executable
        /// </summary>
        Execute = 0x10,
        /// <summary>
        /// Marks the region of virtual memory as readable and/or executable
        /// </summary>
        ExecuteRead = 0x20,
        /// <summary>
        /// Marks the region of virtual memory as readable, writable and/or executable
        /// </summary>
        ExecuteReadWrite = 0x40,
        /// <summary>
        /// Marks the region of virtual memory as readable, copy on write and/or executable
        /// </summary>
        ExecuteWriteCopy = 0x80,
        /// <summary>
        /// Marks the region of virtual memory as guarded
        /// </summary>
        Guard = 0x100,
        /// <summary>
        /// Marks the region of virtual memory as readable, writable and guarded
        /// </summary>
        ReadWriteGuard = 0x104,
        /// <summary>
        /// Marks the region of virtual memory as non-cacheable 
        /// </summary>
        NoCache = 0x200
    }
}