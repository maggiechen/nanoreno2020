using System.Collections.Generic;

public class Dialogue {
    public int id;
    public string actorName;
    public string dialogueText;
    public int? previousDialogueLineId;
    public List<Dialogue> nextLines = new List<Dialogue>();

    public override string ToString() {
        return $"{actorName}: '{dialogueText}'";
    }
}