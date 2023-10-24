using Game.Pools;
using UnityEngine;

namespace Game.Managers
{
    public class PoolsManager : MonoBehaviour
    {
        public static PoolsManager Instance { get; private set; }

        public AudioPool SfxAudioPool => m_sfxAudioPool;
        public ParticlesPool OrbParticlesPool => m_orbParticlesPool;

        [SerializeField] private AudioPool m_sfxAudioPool;
        [SerializeField] private ParticlesPool m_orbParticlesPool;

        public void ReleaseAllPools()
        {
            m_sfxAudioPool.ReleaseAll();
            m_orbParticlesPool.ReleaseAll();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
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
