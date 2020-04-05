using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class PassengerController : MonoBehaviour {
    [SerializeField]
    List<Sprite> hellHeadSprites = null;
    int headSpriteIndex = 0;
    
    [SerializeField]
    List<Light2D> lightsToEnable = null;

    [SerializeField]
    Sprite hellBodySprite = null;
    [SerializeField]
    float hellOpacity = 1;
    [SerializeField]
    SpriteRenderer headRenderer = null;
    [SerializeField]
    SpriteRenderer bodyRenderer = null;

    public void UseHellSprites() {
        headRenderer.sprite = hellHeadSprites[0];
        bodyRenderer.sprite = hellBodySprite;
        headRenderer.SetAlpha(hellOpacity);
        bodyRenderer.SetAlpha(hellOpacity);
        StartCoroutine(ChangeExpressionRandomly());
        foreach (Light2D light in lightsToEnable) {
            light.DOFade(0.4f, 3f);
        }
    }

    IEnumerator ChangeExpressionRandomly() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(3, 6));
            headSpriteIndex = (headSpriteIndex + 1) % hellHeadSprites.Count;
            headRenderer.sprite = hellHeadSprites[headSpriteIndex];
            if (headSpriteIndex != 0) {
                yield return new WaitForSeconds(Random.Range(0.5f, 2));
                headRenderer.sprite = hellHeadSprites[0];
            }
        }
    }
}
