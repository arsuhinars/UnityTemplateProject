using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesEntity : BaseEntity
    {
        public ParticleSystem System => m_particleSystem;

        private ParticleSystem m_particleSystem;

        protected override void SpawnHandler()
        {
            m_particleSystem.Play();
        }

        protected override void ReleaseHandler() { }

        private void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!m_particleSystem.isPlaying)
            {
                Release();
            }
        }
    }
}
