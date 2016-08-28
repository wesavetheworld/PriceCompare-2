using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySuperMarket.ClassLibrary
{
    public class SortedQueue<T> where T : IComparable
    {
        private T[] _queue;

        public SortedQueue(int limit)
        {
            _queue = new T[limit];
        }

        public T First
        {
            get
            {
                return _queue.Length > 0 ? _queue[0] : default(T);
            }
        }

        public T Last
        {
            get
            {
                return _queue.Length > 0 ? _queue[_queue.Length] : default(T);
            }
        }

        public void Add(T item)
        {
            if (_queue.Count() == 0)
            {
                _queue[0] = item;
                return;
            }
            else
            {
                for (int i = 0; i < _queue.Length; i++)
                {
                    if (item.CompareTo(_queue[i]) <= 0)
                    {
                        AdvanceOne(i, ref _queue); // is it smart to use reference?
                        _queue[i] = item;
                        continue;
                    }
                    if (item.CompareTo(_queue[i]) > 0 && i + 1 <= _queue.Length)
                    {
                        AdvanceOne(i + 1, ref _queue); // is it smart to use reference?
                        _queue[i + 1] = item;
                        continue;
                    }
                }
            }
        }

        // advance each item after index (inclusive) forward one count
        public void AdvanceOne(int index, ref T[] queue)
        {
            int length = queue.Length;
            if (!(length <= 0) && index >= 0 && index <= length)
            {
                for (int i = length - 1; i >= index; i--)
                {
                    if (!(i - 1 < 0))
                    {
                        queue[i] = queue[i - 1];
                    }
                }
            }
        }
    }
}
