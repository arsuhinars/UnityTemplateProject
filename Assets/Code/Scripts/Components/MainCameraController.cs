using Game.Managers;
using Game.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Components
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private Camera m_uiCamera;

        private AudioListener m_uiCameraListener;
        private UniversalAdditionalCameraData m_uiCameraData;

        private void Awake()
        {
            m_uiCameraListener = m_uiCamera.GetComponent<AudioListener>();
            m_uiCameraData = m_uiCamera.GetUniversalAdditionalCameraData();
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
                m_uiCameraListener.enabled = true;
                m_uiCameraData.renderType = CameraRenderType.Base;

                var gameCamera = LevelEntities.Instance.GameCamera;
                gameCamera.gameObject.SetActive(false);
            }
        }

        private void OnLoadingFinished(int levelIndex)
        {
            if (LevelEntities.Instance != null)
            {
                m_uiCameraListener.enabled = false;
                m_uiCameraData.renderType = CameraRenderType.Overlay;

                var gameCamera = LevelEntities.Instance.GameCamera;

                gameCamera.gameObject.SetActive(true);
                gameCamera.GetUniversalAdditionalCameraData().cameraStack.Add(m_uiCamera);
            }
        }
    }
}
