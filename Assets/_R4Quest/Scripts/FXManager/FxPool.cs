using System.Collections.Generic;
using UnityEngine;

namespace _R4Quest.Scripts.FXManager
{
    public class FxPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Stack<GameObject> _pool = new Stack<GameObject>();

        public FxPool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Get()
        {
            if (_pool.Count > 0)
            {
                var instance = _pool.Pop();
                instance.SetActive(true);
                return instance;
            }

            return Object.Instantiate(_prefab, _parent);
        }

        public void Release(GameObject instance)
        {
            instance.SetActive(false);
            _pool.Push(instance);
        }
    }
}