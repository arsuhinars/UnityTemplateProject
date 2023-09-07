using Game.Entities;
using UnityEngine;

namespace Game.Utils
{
    public class GlobalEntities : MonoBehaviour
    {
        public static PlayerEntity Player => m_instance.m_player;

        private static GlobalEntities m_instance;

        [SerializeField] private PlayerEntity m_player;

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
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
