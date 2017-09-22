using System;

namespace App
{

    class Program
    {
        private static string srcPath;
        private static string destPath;

        static void Main(string[] args)
        {
            if (args.Length != 2)
             throw new ArgumentException();

            //srcPath = @"d:\mpp\src";//args[0];
            //destPath = @"d:\mpp\dest";//args[1];
            srcPath = args[0];
            destPath = args[1];
            ParallelCopying pc = new ParallelCopying(srcPath, destPath);
            pc.Copy();
            Console.WriteLine(pc.GetCopiedFilesCount());
            Console.ReadLine();            
        }
    }
}
