using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class StoryController : MonoBehaviour {
    void Start() {
        using (var reader = new StreamReader("Assets/Data/Chapter.json")) {
            string json = reader.ReadToEnd();
            Chapter chapter = JsonConvert.DeserializeObject<Chapter>(json);
            Debug.Log(chapter);

            // Dialogue dialogue = JsonConvert.DeserializeObject<Dialogue>("{\"id\":1, \"actorName\": \"asdf\", \"dialogueText\":\"haha\"}");
            // Debug.LogError(dialogue);
        }
        
    }
}
