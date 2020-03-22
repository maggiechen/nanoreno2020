using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightbulbsController : MonoBehaviour {
    [SerializeField]
    List<Light2D> lightbulbs = null;
    List<float> lightbulbIntensities = new List<float>();
    Coroutine flicker;
    void Start() {
        foreach (Light2D light in lightbulbs) {
            lightbulbIntensities.Add(light.intensity);
        }
    }

    public void StartFlickering() {
        flicker = StartCoroutine(Flicker());
    }

    public void StopFlickering() {
        if (flicker != null) {
            StopCoroutine(flicker);
        }
        SetAllLightsIntensityScaled(1f);
    }

    IEnumerator Flicker() {
        while (true) {
            SetAllLightsIntensityScaled(0.1f);
            yield return new WaitForSeconds(0.05f);
            SetAllLightsIntensityScaled(0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SetAllLightsIntensityScaled(float intensity) {
        for (int i = 0; i < lightbulbs.Count; i++) {
            Light2D light = lightbulbs[i];
            light.intensity = lightbulbIntensities[i] * intensity;            
        }
    }
}
