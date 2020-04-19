using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    public Camera mainCamera;
    public float currentZoom = -9;
    private const float MAX_ZOOM = -9;

    private const float ZOOM_THRESHOLD = 0.92f;

    private Transform zoomTarget;
    private void Awake() {
        zoomTarget = FindObjectOfType<ThingToProtect>().transform;
    }

    private void LateUpdate() {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(zoomTarget.position);
        if(Mathf.Abs(viewportPoint.x) > ZOOM_THRESHOLD || Mathf.Abs(viewportPoint.y) > ZOOM_THRESHOLD) {
            currentZoom -= Time.deltaTime*4;
        } else if (currentZoom < MAX_ZOOM){
            currentZoom += Time.deltaTime * 3f;
        }
    }
}
