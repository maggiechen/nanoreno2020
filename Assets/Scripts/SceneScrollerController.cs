using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SceneScrollerController : MonoBehaviour {
    [SerializeField]
    private SceneScrollerGroupController groupController;
    [SerializeField]
    private SpriteRenderer spriteRenderer1;
    [SerializeField]
    private SpriteRenderer spriteRenderer2;
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float scrollSpeed;
    private float horizontalSize;
    private float originalSpritePositionX;
    void Start() {
        originalSpritePositionX = spriteRenderer1.transform.position.x;
        spriteRenderer1.sprite = sprite;
        spriteRenderer2.sprite = sprite;
        horizontalSize = spriteRenderer1.GetWorldSize().x;
        Vector3 spriteCopyPosition = spriteRenderer1.transform.position;
        spriteCopyPosition.x -= horizontalSize * Mathf.Sign(scrollSpeed);
        spriteRenderer2.transform.position = spriteCopyPosition;

    }

    void FixedUpdate() {
        float deltaPosX = transform.position.x - originalSpritePositionX;
        deltaPosX += scrollSpeed * (groupController != null ? groupController.scrollSpeedMultiplier : 1);
        deltaPosX %= horizontalSize;
        Vector3 newPos = transform.position;
        newPos.x = originalSpritePositionX + deltaPosX;        
        transform.position = newPos;
    }
}
