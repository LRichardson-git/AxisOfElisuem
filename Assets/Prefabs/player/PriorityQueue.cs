

using System.Collections.Generic;



    public class PriorityQueue<T>
    {
        private List<T> items;
        private IComparer<T> comparer;

        public PriorityQueue(IComparer<T> comparer = null)
        {
            this.items = new List<T>();
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public void Enqueue(T item)
        {
            items.Add(item);
            int i = items.Count - 1;
            while (i > 0)
            {
                int j = (i - 1) / 2;
                if (comparer.Compare(items[i], items[j]) >= 0)
                {
                    break;
                }
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
                i = j;
            }
        }

        public T Dequeue()
        {
            int lastIndex = items.Count - 1;
            T frontItem = items[0];
            items[0] = items[lastIndex];
            items.RemoveAt(lastIndex);

            lastIndex--;
            int i = 0;
            while (true)
            {
                int leftChild = i * 2 + 1;
                int rightChild = i * 2 + 2;
                if (leftChild > lastIndex)
                {
                    break;
                }
                int j = leftChild;
                if (rightChild <= lastIndex && comparer.Compare(items[rightChild], items[leftChild]) < 0)
                {
                    j = rightChild;
                }
                if (comparer.Compare(items[j], items[i]) >= 0)
                {
                    break;
                }
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
                i = j;
            }

            return frontItem;
        }

        public int Count
        {
            get { return items.Count; }
        }
    }