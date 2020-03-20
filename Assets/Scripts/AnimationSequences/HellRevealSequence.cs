using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class HellRevealSequence : AnimationSequenceController {
    [SerializeField]
    float outsideLightIntensity;
    [SerializeField]
    Color outsideColor;
    [SerializeField]
    Light2D outsideLight;
    [SerializeField]
    float insideLightIntensity;
    [SerializeField]
    Color insideColor;
    [SerializeField]
    Light2D insideLight;



    public override void Prep() {

    }

    public override void StartAnimation(Action onAnimationComplete) {
        outsideLight.color = outsideColor;
        outsideLight.DOFade(outsideLightIntensity, 3f);
        insideLight.color = insideColor;
        insideLight.DOFade(insideLightIntensity, 3f).OnComplete(() => {
            onAnimationComplete();
        });
    }

}
