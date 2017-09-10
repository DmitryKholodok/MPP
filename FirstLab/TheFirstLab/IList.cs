using System.Collections.Generic;

namespace FirstLab
{
    public interface IList<T> : IEnumerable<T>
    {
        void Add(T item);
        bool Remove(T item);
        void RemoveAt(int index);
        void Clear();
    }
}
