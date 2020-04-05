using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData: ScriptableObject {
    [SerializeField]
    private List<CharacterForm> forms = null;
    public Sprite textContainerSprite;
    private Dictionary<string, CharacterForm> formMap = new Dictionary<string, CharacterForm>();
    void OnEnable() {
        PopulateMap();
    }
    public void PopulateMap() {
        formMap.Clear();
        for (int i = 0; i < forms.Count; i++) {
            forms[i].PopulateMap();
            formMap[forms[i].formKey] = forms[i];
        }
    }

    public CharacterForm GetForm(string expressionKey) {
        return formMap[expressionKey];
    }
}

[Serializable]
public class CharacterForm {
    [SerializeField]
    public string formKey;
    [SerializeField]
    private List<CharacterExpression> expressions = null;
    public Sprite bodySprite;
    public Sprite accessorySprite; // optional
    private Dictionary<string, CharacterExpression> expressionMap = new Dictionary<string, CharacterExpression>();

    public void PopulateMap() {
        expressionMap.Clear();
        for (int i = 0; i < expressions.Count; i++) {
            expressionMap[expressions[i].expressionKey] = expressions[i];
        }
    }

    public CharacterExpression GetExpression(string expressionKey) {
        return expressionMap[expressionKey];
    }
}

[Serializable]
public class CharacterExpression {
    public string expressionKey = null;
    public Sprite expressionSprite = null;
}