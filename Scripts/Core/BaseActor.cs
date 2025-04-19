using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBlocks.Arc.Core
{
    public abstract class BaseActor : MonoBehaviour
    {
        [SerializeField] private List<BaseComponent> componentsList = new();
        [SerializeField] private bool isSelfUpdate = true;
        private readonly ComponentsDictionary _componentsDictionary = new();
        private bool _isDirtySort = true;
        private List<BaseComponent> _sortedComponents = new();

        public event Action<BaseComponent> OnAdded;
        public event Action<BaseComponent> OnRemoved;

        public void AddToList(BaseComponent value)
        {
            componentsList.Add(value);
        }

        private void Awake()
        {
            InitDictionary();
            CallAwakeInPriorityOrder();
            if (isSelfUpdate)
            {
                StartCoroutine(SelfUpdate());
            }
        }

        private IEnumerator SelfUpdate()
        {
            while (true)
            {
                yield return null;
                Tick();
            }
        }

        public void Tick()
        {
            {
                if (_isDirtySort)
                    SortComponents();

                foreach (var item in _sortedComponents)
                {
                    item.Tick();
                }
            }
        }

        private void InitDictionary()
        {
            _componentsDictionary.Clear();
            foreach (var componentProvider in componentsList)
            {
                Add(componentProvider);
            }

            MarkDirty();
        }

        // public T GetData<T>() where T : class
        // {
        //     foreach (var component in _componentsDictionary.Values)
        //     {
        //         if (component is BaseComponent and T dataComponent)
        //         {
        //             return dataComponent;
        //         }
        //     }
        //
        //     return null;
        // }

        // public IBehavior GetBehavior<T>() where T : IBehavior
        // {
        //     _componentsDictionary.TryGetValue(typeof(T), out var component);
        //     return component as IBehavior;
        // }

        private void CallAwakeInPriorityOrder()
        {
            if (_isDirtySort)
                SortComponents();

            foreach (var component in _sortedComponents)
            {
                component.Init();
            }
        }

        private void MarkDirty() => _isDirtySort = true;

        public void Add<T>(T component) where T : BaseComponent
        {
            component.Register(this);
            _componentsDictionary[component.GetType()] = component;
            MarkDirty();
            OnAdded?.Invoke(component);
        }

        public T Get<T>() where T : BaseComponent
        {
            _componentsDictionary.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        public bool Has<T>() where T : BaseComponent
        {
            return _componentsDictionary.ContainsKey(typeof(T));
        }

        public bool Remove<T>() where T : BaseComponent
        {
            if (!_componentsDictionary.Remove(typeof(T), out var component)) return false;
            OnRemoved?.Invoke(component);
            MarkDirty();
            return true;
        }

        public void RemoveFromList(BaseComponent baseComponent)
        {
            var index = componentsList.IndexOf(baseComponent);
            componentsList.RemoveAt(index);
        }

        //------------------------------------------------------------------------------------------------------

        [ContextMenu("Sort Components")]
        private void SortComponents()
        {
            _sortedComponents = _componentsDictionary.Values.OrderBy(c => c.Priority).ToList();
            _isDirtySort = false;
        }

        [ContextMenu("Find Components")]
        private void FillListEditor()
        {
            componentsList.Clear();
            var c = GetComponents<BaseComponent>();
            foreach (var componentProvider in c)
            {
                componentProvider.Register(this);
                componentsList.Add(componentProvider);
            }

            MarkDirty();
        }
    }
}