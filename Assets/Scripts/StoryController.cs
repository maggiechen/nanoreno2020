using Newtonsoft.Json;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {
    enum StoryControllerState {
        DEFAULT, // default is displaying text
        CHOOSING,

    }

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
    private ChoiceListController choiceListController = null;


    [SerializeField]
    private StoryControllerState storyControllerState;
    void Awake() {
        using (var reader = new StreamReader("Assets/Data/Chapter.json")) {
            string json = reader.ReadToEnd();
            json = $"{{\"dialogueLines\":{json}}}";
            currentChapter = JsonConvert.DeserializeObject<Chapter>(json);
            currentChapter.PrepareStories();
            Debug.Log(currentChapter);
        }

        // Assume we always start with a text dialogue
        storyControllerState = StoryControllerState.DEFAULT;
        DisableAll();
        EnableText();
        SetTextWithCurrentDialogue();
    }

    void SetTextWithCurrentDialogue() {
        dialogueTextMesh.text = currentChapter.currentDialogueText;
        nameTextMesh.text = currentChapter.currentName;
    }

    void SetChoiceWithCurrentDialogue() {
        choiceListController.SetChoices(currentChapter.choices, (int chosenId) => {
            Debug.LogError($"{chosenId} was chosen");
            storyControllerState = StoryControllerState.DEFAULT;
            nextTextButtonArea.enabled = true;
            OnDialogueClicked(chosenId);
        });
    }

    void SetEnd() {
        dialogueTextMesh.text = "[END]";
    }

    void DisableAll() {
        DisableText();
        DisableChoice();
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
        } else {
            throw new Exception($"Unknown dialogue state {currentChapter.state}");
        }
    }
}
