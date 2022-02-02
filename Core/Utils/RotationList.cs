using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    /// <summary>
    /// Une liste de taille fixe. La plus récente valeur ajoutée supprime la plus ancienne valeur
    /// </summary>
    public class RotationList<T> : IList<T>
    {
        private int counter;
        private List<T> values;

        public RotationList(int size)
        {
            values = new(size);
            for (int i = 0; i < size; i++) values.Add(default);
            counter = 0;
        }

        public T this[int index] 
        { 
            get => values[index];
            set => values[index] = value;
        }

        public int Count => values.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            values[counter] = item;
            counter = (counter + 1) % values.Count;
        }

        public void Clear()
        {
            for (int i = 0; i < values.Count; values[i++] = default) ;
        }

        public bool Contains(T item) => values.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
