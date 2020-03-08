using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chapter {

    public Dialogue currentLine;
    public List<Dialogue> choices = new List<Dialogue>();
    public DialogueState state;
    public List<Dialogue> dialogueLines = new List<Dialogue>();

    public string currentName => currentLine.actorName;
    public string currentDialogueText => currentLine.dialogueText;
    public Dictionary<int, Dialogue> dialogueLinesDict = new Dictionary<int, Dialogue>();
    public override string ToString() {
        string output = string.Join("\n", dialogueLines);
        return $"Chapter ({dialogueLines.Count} lines)\n" + output;
    }

    public void PrepareStories() {
        foreach (Dialogue line in dialogueLines) {
            dialogueLinesDict[line.id] = line;
        }

        foreach (Dialogue line in dialogueLines) {
            if (line.nextLineIdsString != null) {
                string[] ids = line.nextLineIdsString.Split(',');
                foreach (string idString in ids) {
                    line.nextLineIds.Add(int.Parse(idString.Trim()));
                }

            }
        }
        Reset();
    }

    public void Reset() {
        currentLine = dialogueLines[0];
    }

    void UpdateState() {
        Debug.LogError($"Currently on dialogue {currentLine.id}");
        if (currentLine.nextLineIds.Count > 1) {
            state = DialogueState.PRESENTING_CHOICE;
        } else if (currentLine.nextLineIds.Count == 1) {
            state = DialogueState.TEXT;            
        } else {
            state = DialogueState.ENDING;
        }
    }

    public void JumpTo(int dialogueId) {
        choices.Clear();
        currentLine = dialogueLinesDict[dialogueId];
        UpdateState();
    }

    public void MoveNext() {
        choices.Clear();
        if (currentLine.nextLineIds.Count > 1) {
            foreach (int lineId in currentLine.nextLineIds) {
                choices.Add(dialogueLinesDict[lineId]);
            }
        } else if (currentLine.nextLineIds.Count == 1) {
            currentLine = dialogueLinesDict[currentLine.nextLineIds[0]];
        } else {
            Debug.LogError("End of story");
        }

        UpdateState();
    }
}