using System;
using UnityEngine;

namespace Game.Utils
{
    [Serializable]
    public class LevelData
    {
        public string SceneName => m_sceneName;
        public Sprite PreviewImage => m_previewImage;

        [SerializeField] private string m_sceneName;
        [SerializeField] private Sprite m_previewImage;
    }
}
