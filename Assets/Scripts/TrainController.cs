
using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour {
    [SerializeField]
    Transform leftDoor = null;
    [SerializeField]
    Transform rightDoor = null;
    Sequence OpenDoors() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(leftDoor.DOMoveX(0, 1f).SetEase(Ease.OutSine));
        sequence.Join(rightDoor.DOMoveX(0, 1f).SetEase(Ease.OutSine));
        return sequence;
    }

    public void PullIntoStation(TweenCallback callback) {
        transform.DOMoveX(0, 3).SetEase(Ease.OutSine).OnComplete(() => {
            OpenDoors().OnComplete(callback);
        });
    }
}
