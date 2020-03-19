using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {
    enum StoryControllerState {
        DEFAULT, // default is displaying text
        CHOOSING,

    }

    [SerializeField]
    private ChapterContainer chapterContainer = null;
    [SerializeField]
    private Chapter currentChapter;

    [SerializeField]
    private Image nextDialogueIcon = null;
    [SerializeField]
    private TextMeshProUGUI dialogueTextMesh = null;
    [SerializeField]
    private TextMeshProUGUI nameTextMesh = null;
    [SerializeField]
    private Button nextTextButtonArea = null;

    [SerializeField]
    private Button replayButton = null;
    [SerializeField]
    private Button mainMenuButton = null;

    [SerializeField]
    private ChoiceListController choiceListController = null;


    [SerializeField]
    private StoryControllerState storyControllerState;
    void Awake() {
        currentChapter = chapterContainer.chapter;
        currentChapter.PrepareStories();
        Debug.Log(currentChapter);
    }
    
    void Start() {
        BeginStory();
    }

    void BeginStory() {
        ResetProgress();
        gameObject.SetActive(false);
        AnimationController.Instance.BeginAnimationSequence("intro", () => {
            gameObject.SetActive(true);
            SetTextWithCurrentDialogue();
        });
    }

    void ResetProgress() {
        // Assume we always start with a text dialogue
        currentChapter.Reset();
        storyControllerState = StoryControllerState.DEFAULT;
        DisableAll();
        EnableText();
        nextTextButtonArea.enabled = true;
    }

    bool playingText = false;
    float TIME_BETWEEN_LETTER_REVEALS = 0.05f;
    Coroutine playTextCoroutine = null;
    void SetTextWithCurrentDialogue() {
        playingText = true;
        dialogueTextMesh.text = currentChapter.currentDialogueText;
        playTextCoroutine = StartCoroutine(PlayText(dialogueTextMesh.text.Length));
        nameTextMesh.text = currentChapter.currentName;
    }

    IEnumerator PlayText(int characterCount) {
        dialogueTextMesh.maxVisibleCharacters = 0;
        while (dialogueTextMesh.maxVisibleCharacters < characterCount) {
            dialogueTextMesh.maxVisibleCharacters++;
            yield return new WaitForSeconds(TIME_BETWEEN_LETTER_REVEALS);
        }
        playingText = false;
    }

    void SetChoiceWithCurrentDialogue() {
        choiceListController.SetChoices(currentChapter.choices, (int chosenId) => {
            storyControllerState = StoryControllerState.DEFAULT;
            nextTextButtonArea.enabled = true;
            OnDialogueClicked(chosenId);
        });
    }

    void SetEnd() {
        dialogueTextMesh.text = "[END]";
        EnableEnd();
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
        if (playingText) {
            StopCoroutine(playTextCoroutine);
            playingText = false;
            dialogueTextMesh.maxVisibleCharacters = currentChapter.currentDialogueText.Length;
            dialogueTextMesh.text = currentChapter.currentDialogueText;
            return;
        }


        if (dialogueId != -1) {
            currentChapter.JumpTo(dialogueId);
        } else {
            currentChapter.MoveNext();
        }

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
            SetEnd();
            nextTextButtonArea.enabled = false;
        } else {
            throw new Exception($"Unknown dialogue state {currentChapter.state}");
        }
    }

    public void Replay() {
        SceneTransitionController.Instance.StartTransition(BeginStory);
    }

    public void LoadMainMenu() {
        SceneTransitionController.Instance.StartSceneTransition(() => {
            SceneManager.LoadScene("Load");
        });
    }


}
