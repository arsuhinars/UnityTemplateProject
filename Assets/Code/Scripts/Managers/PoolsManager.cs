using Game.Components.Pools;
using UnityEngine;

namespace Game.Managers
{
    public class PoolsManager : MonoBehaviour
    {
        private static PoolsManager m_instance;

        public static AudioPool SfxAudioPool => m_instance.m_sfxAudioPool;
        public static ParticlesPool OrbParticlesPool => m_instance.m_orbParticlesPool;

        [SerializeField] private AudioPool m_sfxAudioPool;
        [SerializeField] private ParticlesPool m_orbParticlesPool;

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
