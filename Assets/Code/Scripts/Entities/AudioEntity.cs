using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEntity : BaseEntity
    {
        private AudioSource m_audioSource;

        protected override void SpawnHandler() { }

        protected override void ReleaseHandler() { }

        public void PlayClip(AudioClip clip)
        {
            m_audioSource.PlayOneShot(clip);
        }

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
