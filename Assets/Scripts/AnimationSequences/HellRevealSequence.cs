using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class HellRevealSequence : AnimationSequenceController {
    [SerializeField]
    float outsideLightIntensity = 1;
    [SerializeField]
    Color outsideColor = Color.white;
    [SerializeField]
    Light2D outsideLight = null;
    [SerializeField]
    float insideLightIntensity = 1;
    [SerializeField]
    Color insideColor = Color.white;
    [SerializeField]
    Light2D insideLight = null;
    [SerializeField]
    Light2D hellLight = null;
    [SerializeField]
    private LightbulbsController lightbulbsController = null;

    [SerializeField]
    private LensFlareController lensFlare = null;

    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip audioClip = null;
    [SerializeField]
    List<PassengerController> passengers = null;

    public override void Prep() {

    }

    public override void StartAnimation(Action onAnimationComplete) {
        audioSource.clip = audioClip;
        audioSource.volume = 1;
        audioSource.Play();
        outsideLight.color = outsideColor;
        outsideLight.DOFade(outsideLightIntensity, 3f);
        insideLight.color = insideColor;
        lightbulbsController.StopFlickering();
        hellLight.intensity = 0;
        hellLight.enabled = true;
        hellLight.DOFade(0.5f, 3f);
        lensFlare.sunTransform = hellLight.transform;
        lensFlare.FadeLensFlareScaled(1, 3f);
        lensFlare.transform.SetParent(hellLight.transform); // we switched suns, lol
        lensFlare.transform.localPosition = Vector3.zero;
        lensFlare.SetColours(Color.red);

        foreach (PassengerController passenger in passengers) {
            passenger.UseHellSprites();
        }

        insideLight.DOFade(insideLightIntensity, 3f).OnComplete(() => {
            onAnimationComplete();
        });
    }

}
