using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jupiter.Services
{
    internal static class ExceptionHandler
    {
        internal static void ThrowWin32Exception(string message)
        {
            // Get the error code for the last win32 error
            
            var errorCode = Marshal.GetLastWin32Error();
            
            throw new Win32Exception($"{message} with error code {errorCode}");
        }
    }
}