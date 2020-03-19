using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData: ScriptableObject {
    [SerializeField]
    private List<CharacterForm> forms = null;
    
    private Dictionary<string, CharacterForm> formMap = new Dictionary<string, CharacterForm>();

    public void OnValidate() {
        formMap.Clear();
        for (int i = 0; i < forms.Count; i++) {
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
    private List<CharacterExpression> expressions;
    public Sprite bodySprite;

    private Dictionary<string, CharacterExpression> expressionMap = new Dictionary<string, CharacterExpression>();

    public void OnValidate() {
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