using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jupiter.Shared.Handlers
{
    internal static class ExceptionHandler
    {
        internal static void ThrowWin32Exception(string message)
        {
            // Get the error code associated with the last PInvoke error

            var lastWin32ErrorCode = Marshal.GetLastWin32Error();

            throw new Win32Exception($"{message} with error code {lastWin32ErrorCode}");
        }
    }
}