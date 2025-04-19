using System;
using UnityEngine;

namespace UnityBlocks.Arc.Core
{
    [Serializable]
    public abstract class BaseComponent : MonoBehaviour
    {
        [field: SerializeField] public int Priority { get; set; }
        public BaseActor Actor { get; private set; }

        public void Register(BaseActor value)
        {
            Actor = value;
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
        // public void Start()
        // {
        // }
        //
        // public void Awake()
        // {
        // }
        //
        // public void Update()
        // {
        // }
        //
        // public void FixedUpdate()
        // {
        // }
        //
        // public void LateUpdate()
        // {
        // }
    }
}