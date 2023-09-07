using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesEntity : BaseEntity
    {
        public ParticleSystem System => m_particleSystem;

        private ParticleSystem m_particleSystem;

        protected override void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();

            OnSpawned += SpawnHandler;
            base.Awake();
        }

        private void Update()
        {
            if (!m_particleSystem.isPlaying)
            {
                Release();
            }
        }

        private void SpawnHandler()
        {
            m_particleSystem.Play();
        }
    }
}
