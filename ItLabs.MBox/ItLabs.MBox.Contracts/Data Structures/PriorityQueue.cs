using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Data_Structures
{
    public class PriorityQueue<T> : IEnumerable
    {
        List<T> items;
        List<int> priorities;

        public PriorityQueue()
        {
            items = new List<T>();
            priorities = new List<int>();
        }
        public IEnumerator GetEnumerator() { return items.GetEnumerator(); }
        public int Count { get { return items.Count; } }


        public int Enqueue(T item, int priority)
        {
            if (items.Contains(item))
            {
                return items.Count;
            }
            for (int i = 0; i < priorities.Count; i++) 
            {
                if (priorities[i] < priority) 
                {
                    items.Insert(i, item);
                    priorities.Insert(i, priority);
                    return i;
                }
            }

            items.Add(item);
            priorities.Add(priority);
            return items.Count - 1;
        }

        public T Dequeue()
        {
            T item = items[0];
            priorities.RemoveAt(0);
            items.RemoveAt(0);
            return item;
        }

        public T Peek()
        {
            return items[0];
        }

        public int PeekPriority()
        {
            return priorities[0];
        }
    }
}
