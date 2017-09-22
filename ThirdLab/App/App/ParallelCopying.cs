using System.Threading;
using System.IO;

namespace App
{
    public class ParallelCopying : Copying
    {
        private class CopyingParam
        {
            public string src;
            public string dest;

            public CopyingParam(string src, string dest)
            {
                this.src = src;
                this.dest = dest;
            }
        }

        private int workThreadCount = 0;
        private volatile int filesCount = 0;
        private readonly ManualResetEvent exitEvent = new ManualResetEvent(false);

        public ParallelCopying(string src, string dest)  :   base(src, dest)
        {           
        }

        public override void Copy()
        {
            workThreadCount++;
            try
            {
                DirectoriesCopying();
                FilesCopying();
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }

            ExitCheck();
            exitEvent.WaitOne();
            
        }

        private void DirectoriesCopying()
        {
            foreach (var directory in Directory.GetDirectories(srcPath, "*.*", SearchOption.AllDirectories))
            {
                Interlocked.Increment(ref workThreadCount);
                ThreadPool.QueueUserWorkItem(CreateDirectory, directory.Replace(srcPath, destPath));
            }
        }

        private void FilesCopying()
        {
            foreach (var file in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                filesCount++;
                Interlocked.Increment(ref workThreadCount);
                ThreadPool.QueueUserWorkItem(CopyFile, new CopyingParam(file, file.Replace(srcPath, destPath)));               
            }
        }

        private void ExitCheck()
        {
            if (Interlocked.Decrement(ref workThreadCount) == 0)
                exitEvent.Set();
        }

        private void CopyFile(object o)
        {
            var prms = (CopyingParam)o;
            File.Copy(prms.src, prms.dest, true);
            ExitCheck();
        }

        private void CreateDirectory(object destPath)
        {
            Directory.CreateDirectory((string)destPath);
            ExitCheck();
        }

        public int GetCopiedFilesCount()
        {
            return filesCount;
        }
    }
}
