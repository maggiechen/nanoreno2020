using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
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

    [SerializeField]
    private SunController sunController = null;

    [SerializeField]
    private Light2D outsideLight = null;
    [SerializeField]
    private Light2D insideLight = null;
    
    public override void Prep() {
    }

    public override void StartAnimation(Action callback) {
        float trainDuration = 1.3f;
        float sunDuration = 2f;
        sunController.FadeSun(sunDuration);
        scrollerGroup.TweenToSpeed(0f, Ease.OutBack, trainDuration, () => {OnTrainStopped(callback);});
        
        // DOTween.To(
        //     () => scrollerGroup.scrollSpeedMultiplier, 
        //     x => scrollerGroup.scrollSpeedMultiplier = x, 
        //     0.5f, duration).SetEase(Ease.InOutBack).OnComplete(() => {callback();});
    }

    void OnTrainStopped(Action callback) {
        float lightDuration = 0.7f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(outsideLight.DOFade(0, lightDuration));
        sequence.AppendInterval(0.3f);
        sequence.Append(insideLight.DOFade(0, lightDuration));
        sequence.Play().OnComplete(() => {

            // TODO: flicker here?
            backgroundController.SetSprites(hellTrainInner, hellTrainOuter);
            scrollerGroup.gameObject.SetActive(false);
            hellScrollerGroup.gameObject.SetActive(true);
            callback();
        });
    }
}