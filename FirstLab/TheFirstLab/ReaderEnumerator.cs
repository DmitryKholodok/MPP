using System;
using System.Collections;
using System.Collections.Generic;

namespace FirstLab
{
    class ReaderEnumerator<T> : IEnumerator<T>
    {
        private T[] arr;
        private int currIndex;
        private T currItem;

        public ReaderEnumerator(T[] arr) {
            this.arr = arr;
            currIndex = -1;
        }

        public T Current
        {
            get { return currItem; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (++currIndex >= arr.Length)
            {
                return false;
            }
            else
            {
                currItem = arr[currIndex];
            }
            return true;
        }

        public void Reset()
        {
            currIndex = -1;
        }
    }
}
