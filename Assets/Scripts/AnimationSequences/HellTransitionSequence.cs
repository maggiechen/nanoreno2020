using System;
using UnityEngine;
using DG.Tweening;
public class HellTransitionSequence: AnimationSequenceController {
    [SerializeField]
    private Sprite hellTrainInner = null;
    [SerializeField]
    private Sprite hellTrainOuter = null;

    [SerializeField]
    private BackgroundController backgroundController = null;

    [SerializeField]
    private SceneScrollerGroupController scrollerGroup = null;
    [SerializeField]
    private SceneScrollerGroupController hellScrollerGroup = null;


    public override void Prep() {
        scrollerGroup.scrollSpeedMultiplier = 0;
    }

    public override void StartAnimation(Action callback) {
        scrollerGroup.TweenToSpeed(0, Ease.OutBack, 0.3f, () => {
            callback();
        });
        
        // DOTween.To(
        //     () => scrollerGroup.scrollSpeedMultiplier, 
        //     x => scrollerGroup.scrollSpeedMultiplier = x, 
        //     0.5f, duration).SetEase(Ease.InOutBack).OnComplete(() => {callback();});
    }
}