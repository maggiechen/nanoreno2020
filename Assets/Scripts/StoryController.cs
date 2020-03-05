using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour {
    Chapter currentChapter;
    public Image nextDialogueIcon;
    public TextMeshProUGUI dialogueTextMesh;
    public TextMeshProUGUI nameTextMesh;

    void Awake() {
        using (var reader = new StreamReader("Assets/Data/Chapter.json")) {
            string json = reader.ReadToEnd();
            currentChapter = JsonConvert.DeserializeObject<Chapter>(json);
            currentChapter.PrepareStories();
            Debug.Log(currentChapter);
        }
        SetTextWithCurrentDialogue();
    }

    void SetTextWithCurrentDialogue() {
        dialogueTextMesh.text = currentChapter.currentName;
        nameTextMesh.text = currentChapter.currentDialogueText;
    }

    public void OnDialogueClicked() {
        currentChapter.MoveNext();
        Debug.LogError(currentChapter.state);
        SetTextWithCurrentDialogue();
    }
}
