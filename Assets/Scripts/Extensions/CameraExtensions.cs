using UnityEngine;
public static class CameraExtensions {
    public static Vector3 NormalizedScreenToWorldPoint(this Camera camera, Vector3 point) {
        return camera.ScreenToWorldPoint(new Vector3(point.x * Screen.width, point.y * Screen.height, point.z));
    }
}