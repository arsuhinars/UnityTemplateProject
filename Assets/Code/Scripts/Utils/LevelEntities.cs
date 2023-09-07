using Game.Entities;
using UnityEngine;

namespace Game.Utils
{
    public class LevelEntities : MonoBehaviour
    {
        public static Transform PlayerSpawnPoint => m_instance.m_playerSpawnPoint;
        public static OrbEntity[] Orbs => m_instance.m_orbs;

        private static LevelEntities m_instance;

        [SerializeField] private Transform m_playerSpawnPoint;
        
        private OrbEntity[] m_orbs;

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }

            m_orbs = FindObjectsByType<OrbEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }   
        }
    }
}
