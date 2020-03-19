using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Dialogue {
    public int id;
    public string actorName;
    public string dialogueText;
    public string nextLineIdsString;
    public List<int> nextLineIds = new List<int>();
    public HashSet<int> nextLineIdSet= new HashSet<int>();
    public List<Change> changeList = new List<Change>();
    public override string ToString() {
        return $"{actorName}: '{dialogueText}'";
    }
}