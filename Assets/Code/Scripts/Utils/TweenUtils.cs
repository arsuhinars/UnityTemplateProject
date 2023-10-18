using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Game.Utils
{
    public static class TweenUtils
    {
        public static TweenerCore<float, float, FloatOptions> DOAlpha(
            this CanvasGroup canvasGroup,
            float endAlpha,
            float duration
        ) {
            return DOTween.To(() => canvasGroup.alpha, (a) => canvasGroup.alpha = a, endAlpha, duration);
        }
    }
}
