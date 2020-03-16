using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransitionController : MonoBehaviour {
    public static SceneTransitionController Instance;
    [SerializeField]
    private RectTransform imageRectTransform = null;

    private float tweenSpeed = 0.5f;

    float width;
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        width = imageRectTransform.GetWidth();
        SetToHidePosition();
    }

    void SetToHidePosition() {
        imageRectTransform.localPosition = new Vector3(-width, 0, 0);
    }

    public void StartSceneTransition(Action sceneLoadCallback) {
        imageRectTransform.DOLocalMoveX(0, tweenSpeed).SetEase(Ease.OutSine).OnComplete(() => {
            sceneLoadCallback();
            StartCoroutine(Uncover());
        });
    }

    public void StartTransition(Action callback) {
        imageRectTransform.DOLocalMoveX(0, tweenSpeed).SetEase(Ease.OutSine).OnComplete(() => {
            callback();
            imageRectTransform.DOLocalMoveX(width, tweenSpeed).SetEase(Ease.InSine).OnComplete(() => {
                SetToHidePosition();
            });
        });
    }

    IEnumerator Uncover() {
        yield return new WaitForSeconds(tweenSpeed);
        imageRectTransform.DOLocalMoveX(width, tweenSpeed).SetEase(Ease.InSine).OnComplete(() => {
            SetToHidePosition();
        });
    }
}
