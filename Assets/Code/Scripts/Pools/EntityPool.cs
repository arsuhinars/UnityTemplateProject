using Game.Entities;
using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pools
{
    public class EntityPool<T> : MonoBehaviour where T : BaseEntity
    {
        [SerializeField] private T m_prefab;
        [SerializeField] private int m_poolSize = 10;
        [Space]
        [SerializeField] private bool m_releaseOnGameStart = true;
        [SerializeField] private bool m_releaseInMainMenu = true;

        private LinkedList<T> m_activeEntities;
        private LinkedList<T> m_inactiveEntities;

        public T SpawnEntity()
        {
            var node = m_inactiveEntities.First;
            if (node == null)
            {
                return null;
            }

            m_inactiveEntities.Remove(node);
            m_activeEntities.AddLast(node);

            node.Value.Spawn();

            return node.Value;
        }

        public void ReleaseAll()
        {
            foreach (var entity in m_activeEntities)
            {
                entity.Release();
                m_inactiveEntities.AddLast(entity);
            }

            m_activeEntities.Clear();
        }

        private IEnumerator Start()
        {
            GameEventsManager.Instance.OnMainMenuOpened += OnMainMenuOpened;
            GameEventsManager.Instance.OnStarted += OnGameStarted;

            m_activeEntities = new();
            m_inactiveEntities = new();

            for (int i = 0; i < m_poolSize; i++)
            {
                var entity = Instantiate(m_prefab, transform);
                m_inactiveEntities.AddLast(entity);
            }

            yield return null;

            foreach (var entity in m_inactiveEntities)
            {
                entity.Release();
            }
        }

        private void OnDestroy()
        {
            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.OnMainMenuOpened -= OnMainMenuOpened;
                GameEventsManager.Instance.OnStarted -= OnGameStarted;
            }
        }

        private void Update()
        {
            var it = m_activeEntities.First;
            while (it != null)
            {
                var next = it.Next;
                if (!it.Value.IsSpawned)
                {
                    m_activeEntities.Remove(it);
                    m_inactiveEntities.AddLast(it);
                }

                it = next;
            }
        }

        private void OnMainMenuOpened()
        {
            if (m_releaseInMainMenu)
            {
                ReleaseAll();
            }
        }

        private void OnGameStarted()
        {
            if (m_releaseOnGameStart)
            {
                ReleaseAll();
            }
        }
    }
}
