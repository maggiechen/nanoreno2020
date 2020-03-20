using System;
using UnityEngine;
using DG.Tweening;
public class IntroAnimationSequence: AnimationSequenceController {

    [SerializeField]
    private SceneScrollerGroupController scrollerGroup = null;

    float duration = 5f;

    public override void Prep() {
        scrollerGroup.scrollSpeedMultiplier = 0;
    }

    public override void StartAnimation(Action callback) {
        DOTween.To(
            () => scrollerGroup.scrollSpeedMultiplier, 
            x => scrollerGroup.scrollSpeedMultiplier = x, 
            0.5f, duration).SetEase(Ease.InOutBack).OnComplete(() => {callback();});
    }
}