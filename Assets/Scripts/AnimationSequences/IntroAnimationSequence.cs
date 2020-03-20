using System;
using UnityEngine;
using DG.Tweening;
public class IntroAnimationSequence: AnimationSequenceController {

    [SerializeField]
    private SceneScrollerGroupController scrollerGroup = null;

    public override void Prep() {
        scrollerGroup.scrollSpeedMultiplier = 0;
    }

    public override void StartAnimation(Action callback) {
        scrollerGroup.TweenToSpeed(0.5f, Ease.InOutBack, 5f, callback);
    }
}