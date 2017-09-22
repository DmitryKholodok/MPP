
using System;
using System.Collections;
using System.Collections.Generic;

namespace FirstLab
{
    public class DynamicList<T> : IList<T> 
    {
        private const int DEFAULT_SIZE = 8;
        private const double SPACE_KOEFF = 2.5;

        private int capacity;
        private T[] arr;
        private int nextIndex;

        public int Count {
            get { return nextIndex; }
        }

        public int Capacity
        {
            get { return capacity; }
        }
        

        public T this[int i]
        {
            get
            {
                CheckIndex(i);
                return arr[i];
            } 
            set
            {
                CheckIndex(i);
                arr[i] = value;
            }
        }

        private void CheckIndex(int i)
        {
            if (!isCorrectIndex(i))
                throw new IndexOutOfRangeException("Incorrect index");                   
        }

        private bool isCorrectIndex(int i)
        {
            return (i >= 0 && i < nextIndex);
        }

        public DynamicList() : this (DEFAULT_SIZE) {
        }

        public DynamicList(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("The capacity should be > 0");

            InitArr(capacity);
        }

        private void InitArr(int capacity)
        {
            this.capacity = capacity;
            arr = new T[capacity];
            nextIndex = 0;
        }

        public void Add(T item)
        {
            if (IsLittleSpace()) ExpandList();
            arr[nextIndex++] = item;
        }
        
        private bool IsLittleSpace()
        {
            return nextIndex > capacity - 1;
        }

        private void ExpandList()
        {
            capacity = (capacity * 3) / 2 + 1;           
            arr = GetNewArr(capacity);           
        }

        private void CompressList()
        {
            capacity = capacity / 2;
            arr = GetNewArr(capacity);
        }

        // Creates a new array and fills it with values of the old array
        private T[] GetNewArr(int capacity)
        {
            T[] newArr = new T[capacity];
            for (int i = 0; i < nextIndex; i++)
            {
                newArr[i] = arr[i];
            }
            return newArr;
        }

        public void Clear()
        {
            InitArr(DEFAULT_SIZE);
        }

        public bool Remove(T item)
        {
            int i = GetItemPosInArr(item);
            if (i == -1) return false;
            Removing(i); 
            return true;
        }

        // find item position in the array and return the number
        // or -1 if there is no such a element
        private int GetItemPosInArr(T item)
        {
            return Array.IndexOf(arr, item, 0, 1);            
        }

        private void Removing(int fromIndex)
        {
            ShiftArrayLeft(fromIndex);
            if (IsALotOfFreeSpace()) CompressList();
        }

        private void ShiftArrayLeft(int fromPos)
        {
            for (int i = fromPos; i < nextIndex - 1; i++)
                arr[i] = arr[i + 1];
            nextIndex--;
        } 

        private void ShiftArrayRight(int fromPos)
        {
        }

        // It determines whether the array should be narrowed
        private bool IsALotOfFreeSpace()
        {
            if (capacity > nextIndex * SPACE_KOEFF && capacity > DEFAULT_SIZE) return true;
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= nextIndex)
                throw new IndexOutOfRangeException("There is no such index in this array");

            Removing(index);
        }

        public override string ToString()
        {
            String str = "";
            for(int i = 0; i < nextIndex; i++)
            {
                str += arr[i].ToString() + " ";
            }
            Console.WriteLine(str + " capacity = " + capacity + " count = " + Count);
            return str;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ReaderEnumerator<T>(arr);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
