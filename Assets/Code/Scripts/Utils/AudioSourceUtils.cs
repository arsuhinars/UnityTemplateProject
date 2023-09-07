using UnityEngine;

namespace Game.Utils
{
    public static class AudioSourceUtils
    {
        public static void PlayRandomClip(this AudioSource source, AudioClip[] clips)
        {
            if (clips.Length > 0)
            {
                source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            }
        }
    }
}
