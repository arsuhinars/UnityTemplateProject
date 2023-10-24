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
            var particles = PoolsManager.Instance.OrbParticlesPool.SpawnEntity();
            if (particles != null)
            {
                particles.transform.position = transform.position;
            }

            PoolsManager.Instance.SfxAudioPool.PlayClipAt(m_pickupClip, transform.position);
        }
    }
}
