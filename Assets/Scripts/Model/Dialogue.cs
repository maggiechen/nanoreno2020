using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType {
    CONVERSATION = 0,
    ANIMATION_TRIGGER,
    END,
}

public enum StoryPointType {
    NONE = 0,
    FRIENDSHIP,
    ROMANCE,
    BAD
}

[Serializable]
public class Dialogue {
    public int id;
    public string actorName;
    public string dialogueText;
    public string nextLineIdsString;
    public DialogueType dialogueType;
    public StoryPointType storyPointType;
    public List<int> nextLineIds = new List<int>();
    public HashSet<int> nextLineIdSet= new HashSet<int>();
    public List<Change> changeList = new List<Change>();
    public override string ToString() {
        return $"[{id}]{actorName}: '{dialogueText}'";
    }
}