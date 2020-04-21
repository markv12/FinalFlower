using UnityEngine;

public class ScaleThrobber : MonoBehaviour
{
    public Transform mainTransform;
    public float speed;
    public float amount;

    private Vector3 normalScale;

    private void Awake() {
        normalScale = mainTransform.localScale;
    }

    void Update()
    {
        float percent = 1 + (Mathf.Sin(Time.unscaledTime*speed)*amount);
        mainTransform.localScale = normalScale * percent;
    }
}
