using System;
using UnityEngine;

namespace Game.Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public event Action OnSpawned;
        public event Action OnReleased;

        public bool IsSpawned { get; private set; }

        public void Spawn()
        {
            if (IsSpawned)
            {
                return;
            }

            IsSpawned = true;
            gameObject.SetActive(true);

            SpawnHandler();
            OnSpawned?.Invoke();
        }

        public void Release()
        {
            if (!IsSpawned)
            {
                return;
            }

            IsSpawned = false;
            gameObject.SetActive(false);

            ReleaseHandler();
            OnReleased?.Invoke();
        }

        protected virtual void Start()
        {
            IsSpawned = true;
            Release();
        }

        protected abstract void SpawnHandler();

        protected abstract void ReleaseHandler();
    }
}
