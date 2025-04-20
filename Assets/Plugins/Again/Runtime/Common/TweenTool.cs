using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Again.Runtime.Common
{
    public static class TweenTool
    {
        public static void AddTween(List<Tween> tweens, Tween tween, Action onComplete = null)
        {
            tweens.Add(tween);
            tween.OnComplete(() =>
            {
                tweens.Remove(tween);
                onComplete?.Invoke();
            });
        }
    }
}