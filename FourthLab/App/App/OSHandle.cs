using System;
using System.Runtime.InteropServices;

namespace App
{
    public class OSHandle : IDisposable
    {
        private bool disposedValue = false;
        public IntPtr Handle { get; set; }

        public OSHandle() { }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { } // managed resources

                //  unmanaged
                if (Handle != IntPtr.Zero)
                {   
                    if (!CloseHandle(Handle))
                    {
                        throw new Exception("Impossible to close the handle!");
                    }
                }

                disposedValue = true;
            }
        }

         ~OSHandle()
         {
            Dispose(false); // just unmanaged resourecs
         }

        public void Dispose()
        {
            Dispose(true); // managed & unmanaged resources
            GC.SuppressFinalize(this); // cancels the finalizer
        }
    }
}
