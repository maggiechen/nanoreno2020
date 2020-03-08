using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChoiceListController : MonoBehaviour {

    public CanvasGroup canvasGroup;
    [SerializeField]
    private List<ChoiceController> choiceList = null;
    [SerializeField]
    private RectTransform choiceListRectTransform = null;
    [SerializeField]
    private ChoiceController choicePrefab = null;
    // Start is called before the first frame update
    void Awake() {
        // delete any test data
        ClearChoices();
    }

    void ClearChoices() {
        foreach (ChoiceController choiceController in choiceList) {
            Destroy(choiceController.gameObject);
        }
        choiceList.Clear();
    }

    // I would do row reuse but also we're only going to have 2-4 choices anyway so it seems like over optimizing
    public void SetChoices(List<Dialogue> choices, Action<int> onChoiceSelectedCallback) {
        ClearChoices();
        foreach (Dialogue dialogue in choices) {
            ChoiceController newChoiceController = Instantiate(choicePrefab);
            newChoiceController.transform.SetParent(transform);
            choiceList.Add(newChoiceController);
            newChoiceController.SetChoice(dialogue, onChoiceSelectedCallback);
        }

        // the elements will overlap unless I tell Unity to recalculate :')
        LayoutRebuilder.ForceRebuildLayoutImmediate(choiceListRectTransform);
    }
}
