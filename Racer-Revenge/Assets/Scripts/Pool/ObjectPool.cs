using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private T type;
        private GameObject prefab;
        private Queue<T> queue = new Queue<T>();
        private List<T> list = new List<T>();
        private PoolController poolController;
        public Action<T> OnGetObject;
        public Action<T> OnReturnObject;
        //create new pool
        public ObjectPool(PoolController poolController, GameObject prefab, int amount)
        {
            this.poolController = poolController;
            this.prefab = prefab;
            if (this.prefab != null)
            {
                int size = Mathf.Max(1, amount);
                for (int i = 0; i < size; ++i)
                {
                    Initialize(i);
                }
            }
        }
        private void Initialize(int index)
        {
            T item = GameObject.Instantiate(prefab).GetComponent<T>();
            item.transform.SetParent(poolController.transform);
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
            item.name = "Pool Item";
            queue.Enqueue(item);
            list.Add(item);
        }
        public T Get()
        {
            if (queue.Count == 0)
            {
                Initialize(queue.Count);
            }
            T item = queue.Dequeue();
            item.name = "Item";
            return item;
        }
        public void Return(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(poolController.transform);
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.name = "Free " + typeof(T);
            if (!queue.Contains(item))
            {
                queue.Enqueue(item);
            }
        }
        public void Reset()
        {
            for (int i = 0; i < list.Count; i++)
            {
                Return(list[i]);
            }
        }
    }

}