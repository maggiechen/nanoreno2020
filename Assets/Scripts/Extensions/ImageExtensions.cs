using UnityEngine;
using UnityEngine.UI;
public static class ImageExtensions {
    public static void SetAlpha(this Image img, float alpha) {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}