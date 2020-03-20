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

    void SayThings() {
        textMesh.text = flavourTexts[textIndex % flavourTexts.Count];
        textMesh.maxVisibleCharacters = 0;
        speechBubble.SetAlpha(0);
        speechBubble.DOFade(1, animDuration).SetEase(Ease.OutSine);
        transform.DOScale(Vector3.one, animDuration).OnComplete(() => {
            StartCoroutine(RevealText(textMesh.text.Length));
        }).SetEase(Ease.OutBack);
        textIndex++;
    }

    IEnumerator RevealText(int characterCount) {    
        while (textMesh.maxVisibleCharacters < characterCount) {
            textMesh.maxVisibleCharacters++;
            yield return new WaitForSeconds(StoryController.TIME_BETWEEN_LETTER_REVEALS);
        }
        yield return new WaitForSeconds(1);
        speechBubble.DOFade(0, animDuration).SetEase(Ease.InSine);
        transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InExpo).OnComplete(() => {
            textMesh.text = "";
            Invoke("SayThings", 5);
        });

    }
}
