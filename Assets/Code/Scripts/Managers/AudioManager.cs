using UnityEngine;
using UnityEngine.Audio;

namespace Game.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private const string SFX_VOLUME_MIXER_KEY = "SfxVolume";
        private const string SFX_VOLUME_PREFS_KEY = "SfxVolume";
        private const string MUSIC_VOLUME_MIXER_KEY = "MusicVolume";
        private const string MUSIC_VOLUME_PREFS_KEY = "MusicVolume";

        public static AudioManager Instance { get; private set; }

        public float SfxVolume
        {
            get => m_sfxVolume;
            set
            {
                m_sfxVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(SFX_VOLUME_PREFS_KEY, m_sfxVolume);
                UpdateMixer();
            }
        }
        public float MusicVolume
        {
            get => m_musicVolume;
            set
            {
                m_musicVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(MUSIC_VOLUME_PREFS_KEY, m_musicVolume);
                UpdateMixer();
            }
        }

        [SerializeField] private AudioMixer m_audioMixer;

        private float m_sfxVolume;
        private float m_musicVolume;

        public static float PercentageVolumeToDb(float volume)
        {
            return Mathf.Clamp(Mathf.Log10(volume) * 40f, -80f, 0f);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            m_sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_PREFS_KEY, 1f);
            m_musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PREFS_KEY, 1f);
            UpdateMixer();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void UpdateMixer()
        {
            m_audioMixer.SetFloat(SFX_VOLUME_MIXER_KEY, PercentageVolumeToDb(m_sfxVolume));
            m_audioMixer.SetFloat(MUSIC_VOLUME_MIXER_KEY, PercentageVolumeToDb(m_musicVolume));
        }
    }
}
