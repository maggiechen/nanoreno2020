using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
public class EndController : MonoBehaviour {
    [SerializeField]
    private AudioSource source = null;
    [SerializeField]
    private AudioClip clip = null;

    [SerializeField]
    private TextMeshProUGUI endText = null;

    [SerializeField]
    private CanvasGroup buttonCanvasGroup = null;
    void Start() {
        source.clip = clip;
        source.Play();
        endText.maxVisibleCharacters = 0;
        buttonCanvasGroup.alpha = 0;
        Invoke("PlayEnding", 2);
    }
    void PlayEnding() {
        endText.DORevealText(endText.text, 4).OnComplete(() => {
            buttonCanvasGroup.DOFade(1, 0.2f);
        });
    }

    public void Replay() {
        SceneTransitionController.Instance.StartSceneTransition(() => {
            SceneManager.LoadScene("Main");
        });
    }

    public void LoadMainMenu() {
        SceneTransitionController.Instance.StartSceneTransition(() => {
            SceneManager.LoadScene("Load");
        });
    }
}
