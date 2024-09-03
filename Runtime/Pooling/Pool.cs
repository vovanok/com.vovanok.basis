using System.Collections.Generic;

namespace UnityBasis.Pooling
{
    public class Pool<T> where T: class
    {
        private const int EXPAND_STEP = 10;

        private readonly Dictionary<T, int> objectIndexes = new();
        private T[] items;
        private int busyCount;
        private IPoolObjectManipulator<T> manipulator;

        public Pool(IPoolObjectManipulator<T> manipulator)
        {
            this.manipulator = manipulator;
        }

        public T Spawn()
        {
            // All busy
            if (items == null || busyCount >= items.Length)
            {
                ExpandBy(EXPAND_STEP);
                return Spawn();
            }

            T targetItem = items[busyCount];
            if (targetItem == null)
                return null;

            objectIndexes[targetItem] = busyCount;
            busyCount++;

            manipulator.SwitchBusyState(targetItem, PoolItemState.Busy);

            return targetItem;
        }

        public bool Despawn(T item)
        {
            if (!objectIndexes.ContainsKey(item))
                return false;

            // Get index of free object
            int itemIndex = objectIndexes[item];
            objectIndexes.Remove(item);

            busyCount--;

            T temp = items[busyCount];
            T itemForFree = items[busyCount] = items[itemIndex];
            items[itemIndex] = temp;
            objectIndexes[temp] = itemIndex;

            manipulator.SwitchBusyState(itemForFree, PoolItemState.Free);

            return true;
        }

        public void Expand(int newCapacity)
        {
            if (items == null)
            {
                items = new T[newCapacity];
                InstantiatePoolItemsOn(0, newCapacity - 1);
            }
            else
            {
                // Expanded objects count isn't enough
                if (items.Length < newCapacity)
                {
                    ExpandBy(newCapacity - items.Length);
                }
            }
        }

        public void ExpandBy(int deltaCapacity)
        {
            if (items == null)
            {
                items = new T[deltaCapacity];
                InstantiatePoolItemsOn(0, deltaCapacity - 1);
            }
            else
            {
                int oldItemsLength = items.Length;
                int newItemsLength = oldItemsLength + deltaCapacity;

                T[] newItems = new T[newItemsLength];

                // Move used object 
                for (int i = 0; i < oldItemsLength; i++)
                    newItems[i] = items[i];

                items = newItems;

                // Add needed objects
                InstantiatePoolItemsOn(oldItemsLength, newItemsLength - 1);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < items.Length; i++)
            {
                manipulator.Destroy(items[i]);
                items[i] = null;
            }

            objectIndexes.Clear();
        }

        private void InstantiatePoolItemsOn(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                T newItem = CreateItem();
                items[i] = newItem;
            }
        }

        private T CreateItem()
        {
            T newItem = manipulator.CreateNew();
            if (newItem != null)
                manipulator.SwitchBusyState(newItem, PoolItemState.Free);

            return newItem;
        }
    }
}