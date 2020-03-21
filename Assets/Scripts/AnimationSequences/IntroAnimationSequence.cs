using System;
using UnityEngine;
using DG.Tweening;
public class IntroAnimationSequence: AnimationSequenceController {

    [SerializeField]
    private SceneScrollerGroupController scrollerGroup = null;

    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClip = null;

    public override void Prep() {
        scrollerGroup.scrollSpeedMultiplier = 0;
    }

    public override void StartAnimation(Action callback) {
        audioSource.clip = audioClip;
        audioSource.Play();
        scrollerGroup.TweenToSpeed(0.5f, Ease.InOutBack, 5f, callback);
    }
}