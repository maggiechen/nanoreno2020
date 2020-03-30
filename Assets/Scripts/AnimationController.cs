
using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class AnimationSequenceController: MonoBehaviour {
    public abstract void Prep();
    public abstract  void StartAnimation(Action onAnimationComplete);
    //public void SkipToEnd();
     
}


public class AnimationController: MonoBehaviour {
    [SerializeField]
    AnimationSequenceController introAnimationSequence = null;
    [SerializeField]
    AnimationSequenceController hellTransitionSequence = null;    
    [SerializeField]
    AnimationSequenceController hellRevealSequence = null;
    [SerializeField]
    ItemRevealSequence itemRevealSequence = null;
    [SerializeField]
    ItemHideSequence itemHideSequence = null;
    
    Dictionary<string, AnimationSequenceController> animationMap = new Dictionary<string, AnimationSequenceController>();

    public static AnimationController Instance;
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        animationMap["intro"] = introAnimationSequence;
        animationMap["trigger_hell_transition"] = hellTransitionSequence;
        animationMap["trigger_hell_reveal"] = hellRevealSequence;
        animationMap["trigger_item_phone_reveal"] = itemRevealSequence;
        animationMap["trigger_item_phone_hide"] = itemHideSequence;
    }

    public void BeginAnimationSequence(string sequenceId, Action onAnimationComplete) {
        AnimationSequenceController sequence = animationMap[sequenceId];
        sequence.Prep();
        sequence.StartAnimation(onAnimationComplete);
    }
}