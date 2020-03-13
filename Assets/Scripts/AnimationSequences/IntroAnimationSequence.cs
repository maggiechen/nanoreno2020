using System;
using UnityEngine;
using DG.Tweening;
public class IntroAnimationSequence: AnimationSequenceController {

    [SerializeField]
    private SpriteRenderer trainSprite = null;

    float duration = 5f;
    float cameraTo2DSceneDistance = 0;

    public override void Prep() {
        cameraTo2DSceneDistance = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 offCameraPoint = Camera.main.NormalizedScreenToWorldPoint(new Vector3(0f, 0.5f, cameraTo2DSceneDistance));
        float width = trainSprite.GetWorldSize().x;
        trainSprite.transform.localPosition = new Vector3(offCameraPoint.x - (width / 2), offCameraPoint.y, 0);
    }

    public override void StartAnimation(Action callback) {
        Vector3 centerPoint = Camera.main.NormalizedScreenToWorldPoint(new Vector3(0.5f, 0.5f, cameraTo2DSceneDistance));
        trainSprite.transform.DOMove(centerPoint, duration).OnComplete(() => {callback();});
        Debug.LogError(trainSprite);
    }
}