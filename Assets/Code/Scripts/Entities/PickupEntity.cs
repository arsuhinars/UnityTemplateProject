using System;
using UnityEngine;

namespace Game.Entities
{
    public class PickupEntity : BaseEntity
    {
        public event Action<GameObject> OnPickedUp;

        [Header("Settings")]
        [SerializeField] private string m_targetTag = string.Empty;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(m_targetTag))
            {
                OnPickedUp?.Invoke(other.gameObject);
                Release();
            }
        }
    }
}
