using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class LensFlareController : MonoBehaviour {
    [SerializeField]
    public Transform sunTransform = null;
    [SerializeField]
    private Transform cameraTransform = null;
    [SerializeField]
    private List<Light2D> lights = null;
    List<float> lightIntensities = new List<float>();

    private List<Vector3> originalLightDisplacements = new List<Vector3>();
    private float originalSunCameraDistance;

    void Awake() {
        foreach (Light2D light in lights) {
            originalLightDisplacements.Add(light.transform.localPosition);
            lightIntensities.Add(light.intensity);
        }
        Vector3 cameraToSun = sunTransform.position - cameraTransform.position;
        cameraToSun.z = 0;
        originalSunCameraDistance = cameraToSun.magnitude;
    }

    void Update() {
        Vector3 cameraToSun = sunTransform.position - cameraTransform.position;
        cameraToSun.z = 0; // project to z plane
        float sunCameraDistance = cameraToSun.magnitude;
        float displacementMultiplier = sunCameraDistance/originalSunCameraDistance;

        float rotation = Vector3.Angle(cameraToSun, Vector3.up);

        // unfortunately Vector3.Angle can only give us the absolute angle difference, so we need to
        // figure out the sign ourselves
        float rotationSign = 1;
        if (cameraToSun.x > 0) {
            rotationSign = -1;
        }

        transform.rotation = Quaternion.AngleAxis(rotation * rotationSign, Vector3.forward);
        for (int i = 0; i < lights.Count; i++) {
            lights[i].transform.localPosition = originalLightDisplacements[i] * displacementMultiplier;
        }
    }

    public void SetColours(Color color) {
        foreach (Light2D light in lights) {
            light.color = color;
        }
    }

    public void FadeLensFlare(float duration) {
        foreach (Light2D light in lights) {
            light.DOFade(0, duration).SetEase(Ease.InBounce);
        }
    }

    public void FadeLensFlareScaled(float intensity, float duration) {
        for (int i = 0; i < lights.Count; i++) {
            Light2D light = lights[i];
            light.DOFade(intensity * lightIntensities[i], duration);
        }
    }
}
