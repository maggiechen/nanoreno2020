using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class SunController : MonoBehaviour {

    [SerializeField]
    private LensFlareController lensFlare = null;

    [SerializeField]
    private List<Light2D> lights = null;
    List<float> lightIntensities = new List<float>();
    
    void Awake() {
        for (int i = 0; i < lights.Count; i++) {
            Light2D light = lights[i];
            lightIntensities.Add(light.intensity);

        }
    }

    public void FadeSun(float duration) {
        lensFlare.FadeLensFlare(duration);
        foreach (Light2D light in lights) {
            light.DOFade(0, duration);
        }
    }
}
