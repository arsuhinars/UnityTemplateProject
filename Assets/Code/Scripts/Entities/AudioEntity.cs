using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEntity : BaseEntity
    {
        public AudioSource Source => m_audioSource;

        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_audioSource.loop = false;
        }

        private void Update()
        {
            if (!m_audioSource.isPlaying)
            {
                Release();
            }
        }
    }
}
