
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IAnimationSequence {
    void Prep();
    void StartAnimation(Action onAnimationComplete);
    //public void SkipToEnd();
     
}


public class AnimationController: MonoBehaviour {
    [SerializeField]
    IntroAnimationSequence introAnimationSequence;
    Dictionary<string, IAnimationSequence> animationMap = new Dictionary<string, IAnimationSequence>();

    public static AnimationController Instance;
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        animationMap["intro"] = introAnimationSequence;
        // TODO: populate here
    }

    public void BeginAnimationSequence(string sequenceId, Action onAnimationComplete) {
        IAnimationSequence sequence = animationMap[sequenceId];
        sequence.Prep();
        sequence.StartAnimation(onAnimationComplete);
    }
}