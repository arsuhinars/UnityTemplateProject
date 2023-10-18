using Game.UI.Elements;
using UnityEngine;

namespace Game.Scriptables
{
    [CreateAssetMenu(fileName = "RoundedRectStyles", menuName = "Game/UI/Rounded Rect Styles")]
    public class RoundedRectStyles : ScriptableObject
    {
        public RoundedRect.StyleData normalStyle;
        public RoundedRect.StyleData hoverStyle;
        public RoundedRect.StyleData pressedStyle;
        public RoundedRect.StyleData disabledStyle;
    }
}
