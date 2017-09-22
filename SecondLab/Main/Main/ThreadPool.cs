using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Main
{
    public class ThreadPool : IThreadPool, IDisposable
    {
        private int threadCount;
        private Thread[] threads;
        private Thread mainHandleThread;
        private ConcurrentQueue<TaskDelegate> taskQueue;
        private Dictionary<int, ManualResetEvent> eventsDictionary;
        private Mutex mutex;

        // ---------------------------------------------------------

        private void InitTaskQueue()
        {
            this.taskQueue = new ConcurrentQueue<TaskDelegate>();
        }

        private void InitWorkingThreads(int threadCount)
        {
            this.threadCount = threadCount;
            this.threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                this.threads[i] = new Thread(ThreadWorking);
                eventsDictionary.Add(threads[i].ManagedThreadId, new ManualResetEvent(false));
                this.threads[i].Name = i.ToString();
                this.threads[i].Start();
            }
        }

        private void InitMutex()
        {
            mutex = new Mutex();
        }

        private void InitMainHandleThread()
        {
            mainHandleThread = new Thread(MainHandlerWork);
            mainHandleThread.Name = "Handle thread";
            mainHandleThread.Start();
        }

        private void InitEventsDictionary()
        {
            eventsDictionary = new Dictionary<int, ManualResetEvent>();
        }

        public ThreadPool(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentException();

            InitMutex();
            InitTaskQueue();
            InitMainHandleThread();
            InitEventsDictionary();
            InitWorkingThreads(threadCount);
        }

        private void ThreadWorking()
        {
            while(true)
            {
                eventsDictionary[Thread.CurrentThread.ManagedThreadId].WaitOne();
                TaskDelegate task = DequeueTask();
                task();
                eventsDictionary[Thread.CurrentThread.ManagedThreadId].Reset();
            }
        }

        private void AddTask(TaskDelegate task)
        {
            taskQueue.Enqueue(task);
        }

        private TaskDelegate DequeueTask()
        {
            bool exit = false;
            TaskDelegate task = null;
            while (!exit)
            {
                exit = taskQueue.TryDequeue(out task);
            }
            return task;
        }

        private void FreeThreadWorking()
        {
            mutex.Lock();
            foreach (var thread in threads)
            {
                if (eventsDictionary[thread.ManagedThreadId].WaitOne(0) == false)
                {
                    eventsDictionary[thread.ManagedThreadId].Set();
                    break;
                }
            }
            mutex.Unlock();
        }

        private void MainHandlerWork()
        {
            while (true)
            {
                if (!taskQueue.IsEmpty)
                {
                    FreeThreadWorking();
                }
            }       
        }

        public void EnqueueTask(TaskDelegate task)
        {
            AddTask(task);
        } 

        public void Dispose()
        {
            mainHandleThread.Abort();
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Abort();
            }
        }
    }
}
