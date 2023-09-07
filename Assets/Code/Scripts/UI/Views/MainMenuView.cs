using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class MainMenuView : BaseUIView
    {
        [SerializeField] private Button m_quitButton;

        private void Start()
        {
            m_quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}
