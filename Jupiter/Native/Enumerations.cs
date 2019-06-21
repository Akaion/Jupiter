using System;

namespace Jupiter.Native
{
    internal static class Enumerations
    {
        [Flags]
        internal enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000
        }
        
        [Flags]
        internal enum FreeType
        {
            Release = 0x8000
        }
    }
}