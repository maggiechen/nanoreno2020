using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChapterContainer))]
public class ChapterScriptableObjectEditor : Editor { 
    public void ReadFromJson(ChapterContainer target, string path = "Assets/Data/Chapter.json") {
        using (var reader = new StreamReader(path)) {
            string json = reader.ReadToEnd();
            json = $"{{\"dialogueLines\":{json}}}";
            Chapter currentChapter = JsonConvert.DeserializeObject<Chapter>(json);


            target.chapter = currentChapter;
            Debug.Log(currentChapter);
        }
    }

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Load Chapter From JSON")) {
            string path = EditorUtility.OpenFilePanel("Overwrite with json", "", "json");
            if (path.Length != 0) {
                ReadFromJson(target as ChapterContainer, path);
            }
        }

        base.OnInspectorGUI();
    }


}