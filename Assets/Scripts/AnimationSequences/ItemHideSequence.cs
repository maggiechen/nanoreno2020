using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemHideSequence : AnimationSequenceController {
    [SerializeField]
    SpriteRenderer itemSprite = null;
    float animDuration = 0.5f;
    
    public override void Prep() {

    }

    public override void StartAnimation(Action onAnimationComplete) {
        itemSprite.material.DOFade(0, animDuration).OnComplete(() => {
            onAnimationComplete();
        });
    }
}
