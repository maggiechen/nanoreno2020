using UnityEngine;
using TMPro;
using DG.Tweening;
public static class TextMeshProUGUIExtensions {
    public static Tween DORevealText(this TextMeshProUGUI textMesh, string targetText, float duration) {
        int targetLen = targetText.Length;
        textMesh.text = targetText;
        textMesh.maxVisibleCharacters = 0;
        return DOTween.To(
            () => textMesh.maxVisibleCharacters,
            x => textMesh.maxVisibleCharacters = x,
            targetLen, duration
        ).SetEase(Ease.Linear);
    }
}