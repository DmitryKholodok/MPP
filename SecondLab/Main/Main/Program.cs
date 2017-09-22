using System;
using System.Threading;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool threadPool = new ThreadPool(4);
            for(int i = 0; i < 10; i++)
            {
                threadPool.EnqueueTask(WriteHello);
            }
            Console.ReadLine();
            threadPool.Dispose();
        }

        static void WriteHello()
        {
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " started to work");
            Thread.Sleep(1000);
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " finished to work");    
        }
    }
}
