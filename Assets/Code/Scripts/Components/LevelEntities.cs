using Game.Entities;
using UnityEngine;

namespace Game.Utils
{
    public class LevelEntities : MonoBehaviour
    {
        public static LevelEntities Instance { get; private set; }

        public Camera GameCamera => m_gameCamera;
        public PlayerEntity Player => m_player;
        public Transform PlayerSpawnPoint => m_playerSpawnPoint;
        public OrbEntity[] Orbs => m_orbs;

        [SerializeField] private Camera m_gameCamera;
        [SerializeField] private PlayerEntity m_player;
        [SerializeField] private Transform m_playerSpawnPoint;
        
        private OrbEntity[] m_orbs;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            m_orbs = FindObjectsByType<OrbEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }   
        }
    }
}
