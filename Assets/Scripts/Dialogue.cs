public class Dialogue {
    public int id;
    public string actorName;
    public string dialogueText;
    public override string ToString() {
        return $"{actorName}: '{dialogueText}'";
    }
}