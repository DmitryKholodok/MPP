using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace App
{
    public abstract class Copying
    {
        protected string srcPath;
        protected string destPath;

        public abstract void Copy();
        
        protected Copying(string src, string dest)
        {
            srcPath = src;
            destPath = dest;
            CheckSrcPath();
        }

        private void CheckSrcPath()
        {
            if (!Directory.Exists(srcPath))
                throw new DirectoryNotFoundException();
        }
    }
}
