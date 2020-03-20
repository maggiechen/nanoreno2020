using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chapter {

    public Dialogue currentLine;
    public List<Dialogue> choices = new List<Dialogue>();
    public DialogueState state;
    public List<Dialogue> dialogueLines = new List<Dialogue>();

    public List<Change> changeList = new List<Change>(); // only used when parsing from json

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

        Reset();
    }

    public void PostJSONDeserialize() {
        foreach (Dialogue line in dialogueLines) {
            if (line.nextLineIdsString != null) {
                string[] ids = line.nextLineIdsString.Split(',');
                foreach (string idString in ids) {
                    if (idString.Length == 0) {
                        continue;
                    }
                    int nextId = int.Parse(idString.Trim());
                    if (!line.nextLineIdSet.Contains(nextId)) { 
                        line.nextLineIds.Add(nextId);
                        line.nextLineIdSet.Add(nextId);
                    }
                }
            }
        }
        foreach (Dialogue line in dialogueLines) {
            dialogueLinesDict[line.id] = line;
        }

        foreach (Change change in changeList) {
            dialogueLinesDict[change.dialogueId].changeList.Add(change);
        }
        changeList.Clear(); // don't need this list anymore
    }

    public void Reset() {
        currentLine = dialogueLines[0];
    }

    void UpdateState() {
        Debug.Log($"Currently on dialogue {currentLine.id}");
        
        if (currentLine.dialogueType == DialogueType.CONVERSATION) {
            if (currentLine.nextLineIds.Count > 1) {
                state = DialogueState.PRESENTING_CHOICE;
            } else if (currentLine.nextLineIds.Count == 1) {
                state = DialogueState.TEXT;            
            } else {
                state = DialogueState.ENDING;
            }
        } else if (currentLine.dialogueType == DialogueType.ANIMATION_TRIGGER) {
            state = DialogueState.ANIMATION;
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
        Debug.LogError(currentLine);

        UpdateState();
    }
}