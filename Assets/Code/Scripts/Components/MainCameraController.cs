using Game.Managers;
using Game.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Components
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(AudioListener))]
    public class MainCameraController : MonoBehaviour
    {
        private Camera m_camera;
        private UniversalAdditionalCameraData m_cameraData;
        private AudioListener m_listener;

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
            m_listener = GetComponent<AudioListener>();
            m_cameraData = m_camera.GetUniversalAdditionalCameraData();
        }

        private void Start()
        {
            LevelManager.Instance.OnLoadingStarted += OnLoadingStarted;
            LevelManager.Instance.OnLoadingFinished += OnLoadingFinished;
        }

        private void OnLoadingStarted(int levelIndex)
        {
            if (LevelEntities.Instance != null)
            {
                m_listener.enabled = true;
                m_cameraData.cameraStack.Remove(LevelEntities.Instance.GameCamera);
            }
        }

        private void OnLoadingFinished(int levelIndex)
        {
            if (LevelEntities.Instance != null)
            {
                m_listener.enabled = false;
                m_cameraData.cameraStack.Insert(0, LevelEntities.Instance.GameCamera);
            }
        }
    }
}
