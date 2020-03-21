using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class LoadingScreenController : MonoBehaviour {
    [SerializeField]
    CanvasGroup tapOutCanvasGroup = null;
    [SerializeField]
    RectTransform creditRectTransform = null;

    [SerializeField]
    TrainController trainController = null;

    [SerializeField]
    SpeechBubbleController speechBubbleController = null;

    [SerializeField]
    Button startButton = null;

    public void OnStartClicked() {
        startButton.enabled = false;
        speechBubbleController.StopSayingThings().OnComplete(() => {
            speechBubbleController.SayMessage("Oh hey train's here!");
        });
        trainController.PullIntoStation(() => {
            SceneTransitionController.Instance.StartSceneTransition(() => {
                SceneManager.LoadScene("Main");
            });
        });

    }

    public void OnCreditsClicked() {
        tapOutCanvasGroup.interactable = true;
        tapOutCanvasGroup.blocksRaycasts = true;
        tapOutCanvasGroup.DOFade(1, 0.3f);
        float height = creditRectTransform.GetHeight();
        
        creditRectTransform.DOAnchorPosY(height/2, 0.2f);
    }

    public void OnTapOutClicked() {
        tapOutCanvasGroup.interactable = false;
        tapOutCanvasGroup.blocksRaycasts = false;
        tapOutCanvasGroup.DOFade(0, 0.2f);
        float height = creditRectTransform.GetHeight();
        creditRectTransform.DOAnchorPosY(-height/2, 0.2f);
    }
}
