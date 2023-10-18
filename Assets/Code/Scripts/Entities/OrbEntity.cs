using Game.Managers;
using UnityEngine;

namespace Game.Entities
{
    public class OrbEntity : PickupEntity
    {
        [Header("Settings")]
        [SerializeField] private AudioClip m_pickupClip;

        private void Awake()
        {
            OnPickedUp += PickupHandler;
        }

        private void PickupHandler(GameObject target)
        {
            var particles = PoolsManager.OrbParticlesPool.SpawnEntity();
            if (particles != null)
            {
                particles.transform.position = transform.position;
            }

            PoolsManager.SfxAudioPool.PlayClipAt(m_pickupClip, transform.position);
        }
    }
}
