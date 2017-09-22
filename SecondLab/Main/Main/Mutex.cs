using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Main
{
    public class Mutex : IMutex
    {
        private int mutex; // 1 - locked, 0 - unlocked;

        public Mutex()
        {
            this.mutex = 0;
        }

        public void Lock()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref mutex, 1, 0) == 0) return;
            }
        }

        public void Unlock()
        {
            Interlocked.CompareExchange(ref mutex, 0, 1);
        }
    }
}
