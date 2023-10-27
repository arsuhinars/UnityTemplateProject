using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class SettingView : BaseView
    {
        [SerializeField] private Slider m_sfxVolumeSlider;
        [SerializeField] private Slider m_musicVolumeSlider;

        protected override void OnShowed()
        {
            UpdateElements();
        }

        protected override void OnHidden() { }

        private void Start()
        {
            m_sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            m_musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        private void OnSfxVolumeChanged(float value)
        {
            AudioManager.Instance.SfxVolume = value;
            UpdateElements();
        }

        private void OnMusicVolumeChanged(float value)
        {
            AudioManager.Instance.MusicVolume = value;
            UpdateElements();
        }

        private void UpdateElements()
        {
            m_sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;
            m_musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        }
    }
}
