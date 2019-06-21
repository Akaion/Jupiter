using System;

namespace Jupiter.Shared
{
    internal static class ExtensionMethods
    {
        internal static IntPtr AddOffset(this IntPtr pointer, ulong offset)
        {
            return (IntPtr) ((ulong) pointer + offset);
        }
    }
}