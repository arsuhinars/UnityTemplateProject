using Game.Entities;
using Game.Managers;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(BaseEntity))]
    public class EntitySpawner : MonoBehaviour
    {
        private BaseEntity m_entity;

        private void Awake()
        {
            m_entity = GetComponent<BaseEntity>();
        }

        private void Start()
        {
            GameEventsManager.Instance.OnStarted += OnGameStarted;
        }

        private void OnDestroy()
        {
            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.OnStarted -= OnGameStarted;
            }
        }

        private void OnGameStarted()
        {
            m_entity.Spawn();
        }
    }
}
