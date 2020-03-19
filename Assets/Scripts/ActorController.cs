
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer bodySpriteRenderer = null;
    [SerializeField]
    private SpriteRenderer headSpriteRenderer = null;
    [SerializeField]
    private CharacterData characterData;
    private string formKey = "normal";
    public void SetForm(string formKey) {
        this.formKey = formKey;
    }

    public void UpdateCharacter(string expressionKey) {
        CharacterForm form = characterData.GetForm(formKey);
        bodySpriteRenderer.sprite = form.bodySprite;
        CharacterExpression expression = form.GetExpression(expressionKey);
        headSpriteRenderer.sprite = expression.expressionSprite;
    }
}
