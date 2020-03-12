using System;
using UnityEngine;
using DG.Tweening;
public class IntroAnimationSequence: AnimationSequenceController {
    [SerializeField]
    private Sprite trainSprite = null;
    public override void Prep() {

    }
    public override void StartAnimation(Action callback) {
        Debug.LogError(trainSprite);
    }
}