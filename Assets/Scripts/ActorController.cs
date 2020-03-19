
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    public string actorName;
    [SerializeField]
    private SpriteRenderer bodySpriteRenderer = null;
    [SerializeField]
    private SpriteRenderer headSpriteRenderer = null;
    [SerializeField]
    private CharacterData characterData = null;

    private string formKey = "normal";
    private string expressionKey = "neutral";

    public void SetForm(string formKey) {
        if (this.formKey != formKey) {
            this.formKey = formKey;
            UpdateCharacter();            
        }
    }
    public void SetExpression(string expressionKey) {
        if (this.expressionKey != expressionKey) {
            this.expressionKey = expressionKey;
            UpdateCharacter();
        }
    }

    public void UpdateCharacter() {
        CharacterForm form = characterData.GetForm(formKey);
        bodySpriteRenderer.sprite = form.bodySprite;
        CharacterExpression expression = form.GetExpression(expressionKey);
        headSpriteRenderer.sprite = expression.expressionSprite;
    }
}
