using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chapter {
    public Dictionary<StoryPointType, int> points = new Dictionary<StoryPointType, int>();
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

    public void PrintFromNode(Dialogue node, int indent = 0) {
        Debug.LogError(GetPrintStringFromNode(node, new HashSet<int>(), new Dictionary<int, string>(), 0));
    }

    public string GetPrintStringFromNode(Dialogue node, HashSet<int> visitedIds, Dictionary<int, string> cachedStrings, int indent = 0) {
        if (!cachedStrings.ContainsKey(indent)) {
            cachedStrings[indent] = "";
            for (int i = 0; i < indent; i++) {
                cachedStrings[indent] += "    ";
            }
        }

        if (visitedIds.Contains(node.id)) {
            return cachedStrings[indent] + "Loop references to " + node + "\n";
        }


        string ret =  cachedStrings[indent] + node + "\n";
        visitedIds.Add(node.id);
        if (node.nextLineIds.Count == 1) {
            ret += GetPrintStringFromNode(dialogueLinesDict[node.nextLineIds[0]], visitedIds, cachedStrings, indent);
        } else {
            foreach (int nextLineId in node.nextLineIds) {
                ret += GetPrintStringFromNode(dialogueLinesDict[nextLineId], visitedIds, cachedStrings, indent + 1) + "\n";
            }
        }
        return ret;
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
        points[StoryPointType.NONE] = 0;
        points[StoryPointType.FRIENDSHIP] = 0;
        points[StoryPointType.ROMANCE] = 0;
        points[StoryPointType.BAD] = 0;
        SetCurrentLine(dialogueLines[0]);

    }


    void UpdateState() {
        Debug.Log($"Currently on dialogue {currentLine.id}");
        
        if (currentLine.dialogueType == DialogueType.CONVERSATION) {
            if (currentLine.nextLineIds.Count > 1) {
                state = DialogueState.PRESENTING_CHOICE;
            } else if (currentLine.nextLineIds.Count == 1) {
                state = DialogueState.TEXT;            
            }
        } else if (currentLine.dialogueType == DialogueType.ANIMATION_TRIGGER) {
            state = DialogueState.ANIMATION;
        } else if (currentLine.dialogueType == DialogueType.END) {
            state = DialogueState.ENDING;
        }
    }

    public void JumpTo(int dialogueId) {
        choices.Clear();
        SetCurrentLine(dialogueLinesDict[dialogueId]);
        UpdateState();
    }

    void SetCurrentLine(Dialogue line) {
        currentLine = line;
        points[currentLine.storyPointType] += 1;
    }
    
    public string GetEndSceneName() {
        int maxPoints = 0;
        StoryPointType winningType = StoryPointType.NONE;
        foreach (KeyValuePair<StoryPointType, int> pair in points) {
            if (pair.Key != StoryPointType.NONE && pair.Value > maxPoints) {
                maxPoints = pair.Value;
                winningType = pair.Key;
            }
        }

        switch (winningType) {
            case StoryPointType.BAD:
                return "BadEnd";
            case StoryPointType.FRIENDSHIP:
                return "FriendEnd";
            case StoryPointType.ROMANCE:
                return "RomanceEnd";
            default:
                throw new Exception("No scene available for this story point type: " + winningType);
        }
    }


    public void MoveNext() {
        choices.Clear();
        if (currentLine.nextLineIds.Count > 1) {
            foreach (int lineId in currentLine.nextLineIds) {
                choices.Add(dialogueLinesDict[lineId]);
            }
        } else if (currentLine.nextLineIds.Count == 1) {
            SetCurrentLine(dialogueLinesDict[currentLine.nextLineIds[0]]);
        } else {
            Debug.LogError("End of story");
        }
        Debug.LogWarning(currentLine);

        UpdateState();
    }
}