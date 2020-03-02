using System.Collections.Generic;
public class Chapter {
    public List<Dialogue> dialogueLines = new List<Dialogue>();
    public override string ToString() {
        string output = string.Join("\n", dialogueLines);
        return $"Chapter ({dialogueLines.Count} lines)\n" + output;
    } 
}