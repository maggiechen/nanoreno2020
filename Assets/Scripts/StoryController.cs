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

    void Start() {
        using (var reader = new StreamReader("Assets/Data/Chapter.json")) {
            string json = reader.ReadToEnd();
            currentChapter = JsonConvert.DeserializeObject<Chapter>(json);
            Debug.Log(currentChapter);
        }
    }

    public void OnDialogueClicked() {
    	Debug.LogError("clicked dialogue");
    }
}
