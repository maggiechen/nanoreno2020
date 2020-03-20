using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public static class Light2DExtensions {
    public static Tween DOFade(this Light2D light, float targetIntensity, float duration) {
        return DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            targetIntensity, duration
        );
    }
}