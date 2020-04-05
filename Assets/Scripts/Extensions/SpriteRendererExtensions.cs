using UnityEngine;
public static class SpriteRendererExtensions {
    public static Vector2 GetWorldSize(this SpriteRenderer spriteRenderer) {
        return spriteRenderer.size * spriteRenderer.transform.localScale;
    }

    public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha) {
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;
    }
}