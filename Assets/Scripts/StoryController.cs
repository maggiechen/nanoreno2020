using System.Collections.Generic;
using System;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {
    public static float TIME_BETWEEN_LETTER_REVEALS = 0.03f;
    enum StoryControllerState {
        DEFAULT, // default is displaying text
        CHOOSING,
    }

    [Header("Data")]
    [SerializeField]
    private ChapterContainer chapterContainer = null;
    [SerializeField]
    private Chapter currentChapter;
    [SerializeField]
    private StoryControllerState storyControllerState;

    [Header("UI References")]
    [SerializeField]
    private Image nextDialogueIcon = null;
    [SerializeField]
    private TextMeshProUGUI dialogueTextMesh = null;
    [SerializeField]
    private Image dialogueTextImage = null;
    [SerializeField]
    private TextMeshProUGUI nameTextMesh = null;
    [SerializeField]
    private Button nextTextButtonArea = null;
    [SerializeField]
    private Button replayButton = null;
    [SerializeField]
    private Button mainMenuButton = null;
    [SerializeField]
    private CanvasGroup canvasGroup = null;

    [Header("UI Tweaks")]
    [SerializeField]
    private float nextButtonAnimationDuration = 0.5f;
    [SerializeField]
    private float fadeDuration = 0.3f;

    [Header("Controllers")]
    [SerializeField]
    private ChoiceListController choiceListController = null;
    [SerializeField]
    private List<ActorController> actors = null;

    [Header("Audio")]
    [SerializeField]
    AudioSource musicSource = null;


    private Dictionary<string, ActorController> actorMap = new Dictionary<string, ActorController>();

    // animation stuff
    Vector3 originalNextDialoguePosition;

    void Awake() {
        currentChapter = chapterContainer.chapter;
        currentChapter.PrepareStories();
        Debug.Log(currentChapter);
        foreach (ActorController actor in actors) {
            actorMap[actor.actorName] = actor;
        }
    }
    
    void Start() {
        originalNextDialoguePosition = nextDialogueIcon.transform.position;
        SetupNextDialogueIcon();
        BeginStory();
    }

    void OnDestroy() {
        translationX.Kill();
        scaleX.Kill();
    }
    
    Tween translationX;
    Tween scaleX;
    void SetupNextDialogueIcon() {
        translationX = nextDialogueIcon.transform.DOLocalMoveX(7, nextButtonAnimationDuration).SetLoops(-1, LoopType.Yoyo);
        scaleX = nextDialogueIcon.transform.DOScaleX(0.8f, nextButtonAnimationDuration).SetLoops(-1, LoopType.Yoyo);
    }

    void BeginStory() {
        ResetProgress();
        gameObject.SetActive(false);
        AnimationController.Instance.BeginAnimationSequence("intro", () => {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, fadeDuration);
            SetTextWithCurrentDialogue();
        });
    }

    void ResetProgress() {
        // Assume we always start with a text dialogue
        currentChapter.Reset();
        storyControllerState = StoryControllerState.DEFAULT;
        dialogueTextImage.SetAlpha(1);
        DisableAll();
        EnableText();
        nextTextButtonArea.enabled = true;
        endOnNextClick = false;
        ApplyChangesOfCurrentDialogue();
    }

    bool _playingText = false;
    bool PlayingText {
        get {
            return _playingText;
        }
        set {
            _playingText = value;
            if (_playingText) {
                translationX.Pause();
                scaleX.Pause();
                nextDialogueIcon.transform.position = originalNextDialoguePosition;
                nextDialogueIcon.transform.localScale = Vector3.one;
                nextDialogueIcon.SetAlpha(0.2f);
            } else {
                translationX.Play();
                scaleX.Play();
                nextDialogueIcon.SetAlpha(1);
            }
        }
    }

    Coroutine playTextCoroutine = null;
    void SetTextWithCurrentDialogue() {
        PlayingText = true;
        dialogueTextMesh.text = currentChapter.currentDialogueText;
        playTextCoroutine = StartCoroutine(PlayText(dialogueTextMesh.text.Length));
        nameTextMesh.text = currentChapter.currentName;
        dialogueTextImage.sprite = actorMap[currentChapter.currentName].GetTextContainerSprite();
    }

    IEnumerator PlayText(int characterCount) {
        dialogueTextMesh.maxVisibleCharacters = 0;
        while (dialogueTextMesh.maxVisibleCharacters < characterCount) {
            dialogueTextMesh.maxVisibleCharacters++;
            yield return new WaitForSeconds(TIME_BETWEEN_LETTER_REVEALS);
        }
        PlayingText = false;
    }

    void SetChoiceWithCurrentDialogue() {
        choiceListController.SetChoices(currentChapter.choices, (int chosenId) => {
            storyControllerState = StoryControllerState.DEFAULT;
            nextTextButtonArea.enabled = true;
            OnDialogueClicked(chosenId);
        });
        dialogueTextImage.sprite = actorMap[currentChapter.choices[0].actorName].GetTextContainerSprite();
    }

    void SetEnd() {
        musicSource.DOFade(0, SceneTransitionController.tweenSpeed);
        SceneTransitionController.Instance.StartSceneTransition(() => {
            SceneManager.LoadScene("BadEnd");
        });
        // nameTextMesh.text = "";
        // dialogueTextImage.SetAlpha(0);
        // nextDialogueIcon.SetAlpha(0);
        // dialogueTextMesh.text = "[END]";
        // EnableEnd();
    }

    void DisableEnd() {
        replayButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    void EnableEnd() {
        replayButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    void DisableAll() {
        DisableText();
        DisableChoice();
        DisableEnd();
    }

    void DisableText() {
        dialogueTextMesh.enabled = false;
        nameTextMesh.enabled = false;
        nextDialogueIcon.enabled = false;
    }

    void EnableText() {
        dialogueTextMesh.enabled = true;
        nameTextMesh.enabled = true;
        nextDialogueIcon.enabled = true;
    }

    void DisableChoice() {
        choiceListController.canvasGroup.alpha = 0;
        choiceListController.canvasGroup.interactable = false;
        choiceListController.canvasGroup.blocksRaycasts = false;
        
    }

    void EnableChoice() {
        choiceListController.canvasGroup.alpha = 1;
        choiceListController.canvasGroup.interactable = true;
        choiceListController.canvasGroup.blocksRaycasts = true;
    }

    public void OnDialogueClicked(int dialogueId=-1) {

        if (PlayingText) {
            StopCoroutine(playTextCoroutine);
            PlayingText = false;
            dialogueTextMesh.maxVisibleCharacters = currentChapter.currentDialogueText.Length;
            dialogueTextMesh.text = currentChapter.currentDialogueText;
            return;
        }

        if (endOnNextClick) {
            End();
            return;
        }

        string oldActor = currentChapter.currentName;
        if (dialogueId != -1) {
            currentChapter.JumpTo(dialogueId);
        } else {
            currentChapter.MoveNext();
        }

        if (currentChapter.state == DialogueState.ANIMATION) {
            HandControlToAnimationController();
            return;
        }

        if (oldActor.Length != 0 && !currentChapter.currentName.Equals(oldActor)) {
            nextTextButtonArea.enabled = false;
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() => {
                UpdateStoryController();
                canvasGroup.DOFade(1, fadeDuration);
                nextTextButtonArea.enabled = true;
            });
        } else {
            UpdateStoryController();
        }
    }

    void HandControlToAnimationController() {
        nextTextButtonArea.enabled = false;
        canvasGroup.DOFade(0, fadeDuration).OnComplete(() => {
            nextTextButtonArea.enabled = true;
            gameObject.SetActive(false);
            AnimationController.Instance.BeginAnimationSequence(currentChapter.currentLine.dialogueText, () => {
                gameObject.SetActive(true);
                canvasGroup.alpha = 0;
                OnDialogueClicked();
                canvasGroup.DOFade(1, fadeDuration);
            });

        });
    }

    void End() {
        DisableAll();
        EnableText();
        SetEnd();
        nextTextButtonArea.enabled = false;
    }

    bool endOnNextClick = false;
    void UpdateStoryController() {
        if (storyControllerState == StoryControllerState.CHOOSING) {
            DisableAll();
            EnableChoice();
            SetChoiceWithCurrentDialogue();
            nextTextButtonArea.enabled = false;
        } else if (currentChapter.state == DialogueState.PRESENTING_CHOICE) {
            DisableAll();
            EnableText();
            SetTextWithCurrentDialogue();
            storyControllerState = StoryControllerState.CHOOSING;
        } else if (currentChapter.state == DialogueState.TEXT) {
            DisableAll();
            EnableText();
            SetTextWithCurrentDialogue();
        } else if (currentChapter.state == DialogueState.ENDING) {
            DisableAll();
            EnableText();
            SetTextWithCurrentDialogue();
            endOnNextClick = true;
        } else {
            throw new Exception($"Unknown dialogue state {currentChapter.state}");
        }

        ApplyChangesOfCurrentDialogue();
    }

    void ApplyChangesOfCurrentDialogue() {
        foreach (Change change in currentChapter.currentLine.changeList) {
            if (change.changeType == ChangeType.EXPRESSION) {
                actorMap[change.actorName].SetForm(change.formKey);
                actorMap[change.actorName].SetExpression(change.expressionKey);
            }
        }
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
