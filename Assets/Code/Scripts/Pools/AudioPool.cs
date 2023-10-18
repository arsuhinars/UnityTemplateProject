using Game.Entities;
using UnityEngine;

namespace Game.Pools
{
    public class AudioPool : EntityPool<AudioEntity>
    {
        public void PlayClipAt(AudioClip clip, Vector3 position)
        {
            var audio = SpawnEntity();
            if (audio != null)
            {
                audio.transform.position = position;
                audio.PlayClip(clip);
            }
        }
    }
}
