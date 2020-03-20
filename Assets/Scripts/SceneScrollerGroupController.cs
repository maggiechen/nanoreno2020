using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SceneScrollerGroupController : MonoBehaviour {
    public float scrollSpeedMultiplier = 0.5f;

    public void TweenToSpeed(float targetScrollSpeedMultiplier, Ease ease, float duration, Action callback) {
        DOTween.To(
            () => scrollSpeedMultiplier, 
            x => scrollSpeedMultiplier = x, 
            targetScrollSpeedMultiplier, duration).SetEase(ease).OnComplete(() => {
                callback();
            });
    }
}
