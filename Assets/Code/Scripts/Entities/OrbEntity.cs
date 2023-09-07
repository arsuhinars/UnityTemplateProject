using Game.Managers;
using UnityEngine;

namespace Game.Entities
{
    public class OrbEntity : PickupEntity
    {
        [Header("Settings")]
        [SerializeField] private AudioClip m_pickupClip;

        protected override void Awake()
        {
            OnPickedUp += PickupHandler;
            base.Awake();
        }

        private void PickupHandler(GameObject target)
        {
            var particles = PoolsManager.OrbParticlesPool.SpawnEntity();
            if (particles != null)
            {
                particles.transform.position = transform.position;
            }

            var audio = PoolsManager.SfxAudioPool.SpawnEntity();
            if (audio != null)
            {
                audio.transform.position = transform.position;
                audio.Source.clip = m_pickupClip;
                audio.Source.Play();
            }
        }
    }
}
