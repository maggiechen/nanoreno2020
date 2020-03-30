using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemRevealSequence : AnimationSequenceController {
    [SerializeField]
    SpriteRenderer itemSprite = null;

    float animDuration = 0.5f;
    Vector3 originalPosition;
    void Awake() {
        originalPosition = transform.position;
        transform.position = originalPosition - new Vector3(0, 100, 0);
        itemSprite.material.DOFade(0, 0);
    }

    public override void Prep() {

    }
    
    public override void StartAnimation(Action onAnimationComplete) {
        Sequence s = DOTween.Sequence();
        s.Append(itemSprite.material.DOFade(1, animDuration));
        s.Join(itemSprite.transform.DOMove(originalPosition, animDuration));
        s.AppendInterval(1.5f);
        s.AppendCallback(() => {
            onAnimationComplete();
        });        
    }
}
