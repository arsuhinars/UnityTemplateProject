using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private Button m_quitButton;

        protected override void OnShowed() { }

        protected override void OnHidden() { }

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
