using System;
using UnityEngine;

namespace UnityBlocks.Arc.Modules.UnityBlocks_ARC.Scripts.Core
{
    [Serializable]
    public abstract class BaseComponent : MonoBehaviour
    {
        [field: SerializeField] public int Priority { get; set; }
        public ActorBase Actor { get; private set; }

        public void Register(ActorBase value)
        {
            Actor = value;
        }

        private void Reset()
        {
            if (TryGetComponent(out ActorBase actorBase))
            {
                actorBase.AddToList(this);
            }
        }

        private void OnDestroy()
        {
            if (TryGetComponent(out ActorBase actorBase))
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