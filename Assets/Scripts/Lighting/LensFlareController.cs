using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LensFlareController : MonoBehaviour {
    [SerializeField]
    private Transform sunTransform = null;
    [SerializeField]
    private Transform cameraTransform = null;
    [SerializeField]
    private List<Light2D> lights = null;

    private List<Vector3> originalLightDisplacements = new List<Vector3>();
    private float originalSunCameraDistance;

    void Awake() {
        foreach (Light2D light in lights) {
            originalLightDisplacements.Add(light.transform.localPosition);
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
}
