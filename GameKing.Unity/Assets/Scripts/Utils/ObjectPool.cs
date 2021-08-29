using System.Collections.Generic;
using UnityEngine;

namespace GameKing.Unity.Utils
{
    public interface IManagedObject
    {
        void PreInit();
        void Release();
    }

    public class ObjectPool<T> where T : Component
    {
        private readonly Stack<T> _objectPool;
        private readonly T _oriObject;
        private readonly string _objName;
        private readonly Transform _parent;
        private readonly int _overAllocateCount;
        private readonly bool _active;
        private readonly bool _isParentSelf;
        private int _allocCount;

        public List<T> PopList { get; private set; }

        public ObjectPool(T oriObject, string objName, Transform parent, int count, int overAllocateCount,
            bool active = false, bool isParentSelf = false)
        {
            _oriObject = oriObject;
            _objName = objName;
            _parent = parent;
            _overAllocateCount = overAllocateCount;
            _objectPool = new Stack<T>();
            PopList = new List<T>();
            _active = active;
            _isParentSelf = isParentSelf;
            _allocCount = 0;

            Allocate(count, active);
        }

        public void Allocate(int allocCount, bool active = false)
        {
            for (var i = 0; i < allocCount; ++i)
            {
                var obj = GameObject.Instantiate<T>(_oriObject, _parent, false);
                obj.name = _objName + _allocCount;
                obj.gameObject.SetActive(active);

                (obj as IManagedObject)?.PreInit();

                _objectPool.Push(obj);
                ++_allocCount;
            }
        }

        public T Pop(bool setActive = true)
        {
            if (_objectPool.Count <= 0)
            {
                Allocate(_overAllocateCount, _active);
            }

            var retObj = _objectPool.Pop();
            PopList.Add(retObj);
            retObj.gameObject.SetActive(setActive);
            return retObj;
        }

        public void Push(T obj, bool setActive = false, bool isRemoveInList = true)
        {
            if (_isParentSelf == false)
                obj.transform.SetParent(_parent);

            obj.gameObject.SetActive(setActive);

            if (_objectPool.Contains(obj))
                return;

            (obj as IManagedObject)?.Release();

            _objectPool.Push(obj);

            if (isRemoveInList)
                PopList.Remove(obj);
        }

        public void Push(T[] objs, bool setActive = false)
        {
            foreach (var obj in objs)
                Push(obj, setActive);
        }

        public void PushAll(bool setActive)
        {
            if (PopList.Count == 0)
                return;

            foreach (var popObj in PopList)
                Push(popObj, setActive, false);

            PopList.Clear();
        }

        public int GetCurPoolCount()
        {
            return _objectPool.Count;
        }
    }
}