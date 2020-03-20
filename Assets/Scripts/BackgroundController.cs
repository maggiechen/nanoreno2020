using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {
    [SerializeField]
    SpriteRenderer trainInside = null;
    [SerializeField]
    SpriteRenderer trainOutside = null;
    public void SetSprites(Sprite insideSprite, Sprite outsideSprite) {
        trainInside.sprite = insideSprite;
        trainOutside.sprite = outsideSprite;
    }
}
