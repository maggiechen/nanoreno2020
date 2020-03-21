using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class SpeechBubbleController : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI textMesh = null;
    [SerializeField]
    Image speechBubble = null;
    [SerializeField]
    List<string> flavourTexts = null;
    int textIndex = 0;
    float animDuration = 0.2f;

    void Start() {
        SayThings();
        transform.localScale = Vector3.zero;
    }

    public void SayMessage(string message) {
        SayThings(message);
    }

    Coroutine revealTextCoroutine;
    void SayThings(string message = null) {
        if (message != null) {
            textMesh.text = message;
        } else {
            textMesh.text = flavourTexts[textIndex % flavourTexts.Count];
        }
        textMesh.maxVisibleCharacters = 0;
        speechBubble.SetAlpha(0);

        speechBubble.DOFade(1, animDuration).SetEase(Ease.OutSine);
        transform.DOScale(Vector3.one, animDuration).OnComplete(() => {
            revealTextCoroutine = StartCoroutine(RevealText(textMesh.text.Length, message == null));
        }).SetEase(Ease.OutBack);
        textIndex++;
    }

    public Tween StopSayingThings() {
        if (revealTextCoroutine != null) {
            StopAllCoroutines();
        }
        return transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InExpo).OnComplete(() => {
            textMesh.text = "";
        });
    }

    IEnumerator RevealText(int characterCount, bool invokeNext = true) {    
        while (textMesh.maxVisibleCharacters < characterCount) {
            textMesh.maxVisibleCharacters++;
            yield return new WaitForSeconds(StoryController.TIME_BETWEEN_LETTER_REVEALS);
        }
        yield return new WaitForSeconds(1);
        speechBubble.DOFade(0, animDuration).SetEase(Ease.InSine);
        transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InExpo).OnComplete(() => {
            textMesh.text = "";
            if (invokeNext) {
                StartCoroutine(WaitAndCall(2));
            }
        });
    }

    IEnumerator WaitAndCall(float seconds) {
        yield return new WaitForSeconds(seconds);
        SayThings();
    }
}
