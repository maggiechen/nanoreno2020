using UnityEngine;
public static class SpriteRendererExtensions {
    public static Vector2 GetWorldSize(this SpriteRenderer spriteRenderer) {
        return spriteRenderer.size * spriteRenderer.transform.localScale;
    }
}