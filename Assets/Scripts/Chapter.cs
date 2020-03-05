using System.Collections.Generic;
using UnityEngine;
public class Chapter {
    private Dialogue currentLine;
    
    public string currentName => currentLine.actorName;
    public string currentDialogueText => currentLine.dialogueText;
    public List<Dialogue> dialogueLines = new List<Dialogue>();
    public Dictionary<int, Dialogue> dialogueLinesDict = new Dictionary<int, Dialogue>();
    public override string ToString() {
        string output = string.Join("\n", dialogueLines);
        return $"Chapter ({dialogueLines.Count} lines)\n" + output;
    }

    public void PrepareStories() {
        foreach (Dialogue line in dialogueLines) {
            dialogueLinesDict[line.id] = line;
        }

        bool firstLine = true;
        foreach (Dialogue line in dialogueLines) {
            if (firstLine) {
                firstLine = false;
                continue;
            }
            Dialogue previousLine;
            if (line.previousDialogueLineId.HasValue) {
                previousLine = dialogueLinesDict[line.previousDialogueLineId.Value];
            } else {
                if (!dialogueLinesDict.ContainsKey(line.id - 1)) {
                    Debug.LogError(line + " references " + (line.id - 1) + " but this line does not exist");
                }
                previousLine = dialogueLinesDict[line.id - 1];
            }
            previousLine.nextLines.Add(line);
        }
        currentLine = dialogueLines[0];
    }

    public void MoveNext() {
        if (currentLine.nextLines.Count > 1) {
            Debug.LogError("Choice");
        } else if (currentLine.nextLines.Count == 1) {
            currentLine = currentLine.nextLines[0];
        } else {
            Debug.LogError("End of story");
        }
    }
}