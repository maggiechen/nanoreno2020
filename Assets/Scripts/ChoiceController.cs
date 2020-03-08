using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceController : MonoBehaviour{
    [SerializeField]
    private Image optionIcon = null;
    [SerializeField]
    private TextMeshProUGUI choiceTextMesh = null;
    [SerializeField]
    private RectTransform choiceTextRectTransform = null;
    
    private Dialogue dialogue;
    private Action<int> onChoiceSelectedCallback;

    public void SetChoice(Dialogue dialogue, Action<int> onChoiceSelectedCallback) {
        this.dialogue = dialogue;
        this.onChoiceSelectedCallback = onChoiceSelectedCallback;
        choiceTextMesh.text = dialogue.dialogueText;
        choiceTextMesh.ForceMeshUpdate();
        choiceTextRectTransform.SetHeight(choiceTextMesh.preferredHeight);
    }

    public void OnChosen() {
        onChoiceSelectedCallback?.Invoke(dialogue.id);
    }

}
