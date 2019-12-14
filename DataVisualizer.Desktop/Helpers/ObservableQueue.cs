using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Helpers
{
    public class ObservableQueue<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly Queue<T> queue = new Queue<T>();
        public int Count
        {
            get => queue.Count;
        }

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            CollectionChanged?.Invoke(this, 
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, queue.Count));
        }

        public T Dequeue()
        {
            var item = queue.Dequeue();
            CollectionChanged?.Invoke(this, 
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, 0));
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queue.GetEnumerator();
        }
        #region Conversions
        public static implicit operator ObservableQueue<T>(T[] array)
        {
            ObservableQueue<T> result = new ObservableQueue<T>();
            foreach (var element in array)
                result.Enqueue(element);
            return result;
        }

        public static implicit operator T[](ObservableQueue<T> queue)
        {
            return queue.ToArray();
        }
        #endregion
    }
}
