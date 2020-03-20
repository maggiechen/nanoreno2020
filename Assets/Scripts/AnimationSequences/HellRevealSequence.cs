﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class HellRevealSequence : AnimationSequenceController {
    [SerializeField]
    float outsideLightIntensity = 1;
    [SerializeField]
    Color outsideColor = Color.white;
    [SerializeField]
    Light2D outsideLight = null;
    [SerializeField]
    float insideLightIntensity = 1;
    [SerializeField]
    Color insideColor = Color.white;
    [SerializeField]
    Light2D insideLight = null;


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