using System;

public enum ChangeType {
    EXPRESSION = 0
}

[Serializable]
public class Change { // very generically named but it's for a small project so :p
    public int dialogueId;
    public ChangeType changeType;
    public string actorName;
    public string formKey;
    public string expressionKey;
}