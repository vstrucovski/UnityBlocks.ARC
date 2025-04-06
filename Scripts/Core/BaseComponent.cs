using System;
using UnityEngine;

namespace UnityBlocks.Arc.Core
{
    [Serializable]
    public abstract class BaseComponent : MonoBehaviour
    {
        [field: SerializeField] public int Priority { get; set; }
        public BaseActor BaseActor { get; private set; }

        public void Register(BaseActor value)
        {
            BaseActor = value;
        }

        private void Reset()
        {
            if (TryGetComponent(out BaseActor actorBase))
            {
                actorBase.AddToList(this);
            }
        }

        private void OnDestroy()
        {
            if (TryGetComponent(out BaseActor actorBase))
            {
                Debug.Log("remove actor");
                actorBase.RemoveFromList(this);
            }
        }

        protected internal virtual void Init()
        {
        }

        protected internal virtual void Tick()
        {
        }

        //block usage of native calls from Unity
        private void Start()
        {
        }

        private void Awake()
        {
        }

        private void Update()
        {
        }
    }
}