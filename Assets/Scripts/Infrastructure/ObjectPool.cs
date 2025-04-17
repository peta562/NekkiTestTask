
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly Func<T> _createFunc;
        private readonly Transform _parent;
        private readonly bool _canGrow;

        public ObjectPool(Func<T> createFunc, int initialSize = 10, bool canGrow = true, Transform parent = null)
        {
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _canGrow = canGrow;
            _parent = parent;

            Preload(initialSize);
        }

        public void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Create();
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            if ( _pool.Count > 0 )
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            if ( _canGrow )
            {
                var obj = Create();
                obj.gameObject.SetActive(true);
                return obj;
            }

            Debug.LogWarning($"[ObjectPool<{typeof(T).Name}>] Pool is empty and can't grow.");
            return null;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }

        private T Create()
        {
            T instance = _createFunc();
            if ( _parent != null )
                instance.transform.SetParent(_parent);

            return instance;
        }
    }
}
