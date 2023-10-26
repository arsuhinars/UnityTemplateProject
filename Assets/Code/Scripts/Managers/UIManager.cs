using Game.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Components")]
        [SerializeField] private Transform m_viewsRoot;
        [Header("Settings")]
        [SerializeField] private string m_initialViewName;
        [Header("Audio")]
        [SerializeField] private AudioSource m_uiAudioSource;
        [SerializeField] private AudioClip m_clickSound;

        private Dictionary<string, UIView> m_viewsByName;
        private UIView m_activeView = null;

        public void SetActiveView(string viewName)
        {
            if (!m_viewsByName.TryGetValue(viewName, out var view))
            {
                Debug.LogError($"View with name \"{viewName}\" was not found");
                return;
            }

            if (m_activeView != null)
            {
                m_activeView.Hide();
            }
            m_activeView = view;
            view.Show();
        }

        public UIView GetView(string viewName)
        {
            if (m_viewsByName.TryGetValue(viewName, out var view))
            {
                return view;
            }

            return null;
        }

        public void PlayClickSound()
        {
            m_uiAudioSource.PlayOneShot(m_clickSound);
        }

        public void PlayClip(AudioClip clip)
        {
            m_uiAudioSource.PlayOneShot(clip);
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
            FindViews();

            foreach (var view in m_viewsByName.Values)
            {
                if (view.name == m_initialViewName)
                {
                    view.Show(true);
                }
                else
                {
                    view.Hide(true);
                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void FindViews()
        {
            m_viewsByName = new();

            for (int i = 0; i < m_viewsRoot.childCount; i++)
            {
                var child = m_viewsRoot.GetChild(i);
                
                if (!child.TryGetComponent<UIView>(out var view))
                {
                    Debug.LogError("View elements in view container must be inherited from BaseUIView");
                    continue;
                }

                if (!m_viewsByName.TryAdd(view.name, view))
                {
                    Debug.LogError("View elements must have unique names");
                }
            }
        }
    }
}
