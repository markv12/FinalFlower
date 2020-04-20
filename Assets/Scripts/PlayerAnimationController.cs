using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    public Transform wheel;
    public Transform head;
    public Transform pupil1;
    private Vector3 pupil1RootPos;
    public Transform pupil2;
    private Vector3 pupil2RootPos;

    public Transform eyeTarget;

    public SpriteRenderer headRenderer;
    public SpriteRenderer eyesRenderer;
    public Sprite headSprite;
    public Sprite headAlarmSprite;
    public Sprite eyesSprite;
    public Sprite eyesAlarmSprite;

    public Vector3 headLowPosition;
    public Vector3 headTopPosition;

    private void Awake() {
        pupil1RootPos = pupil1.localPosition;
        pupil2RootPos = pupil2.localPosition;
    }

    public void RotateWheel(float amount) {
        wheel.Rotate(new Vector3(0, 0, amount*-1.2f));
    }

    public void Jump() {
        this.EnsureCoroutineStopped(ref jumpRoutine);
        this.EnsureCoroutineStopped(ref jumpSubRoutine);
        jumpRoutine = StartCoroutine(_Jump());
    }

    private Coroutine jumpRoutine = null;
    private Coroutine jumpSubRoutine = null;
    private IEnumerator _Jump() {
        Vector3 startPos = head.localPosition;
        Vector3 endPos = headTopPosition;
        jumpSubRoutine = this.CreateAnimationRoutine(
            0.1f,
            delegate (float progress) {
                head.localPosition = Vector3.Lerp(startPos, endPos, progress);
            }
        );
        yield return jumpSubRoutine;
        startPos = headTopPosition;
        endPos = headLowPosition;
        jumpSubRoutine = this.CreateAnimationRoutine(
             0.3f,
             delegate (float progress) {
                 head.localPosition = Vector3.Lerp(startPos, endPos, progress);
             }
         );
        yield return jumpSubRoutine;
    }

    private void Update() {
        pupil1.localPosition = GetClosestPointToEyeTarget(pupil1RootPos);
        pupil2.localPosition = GetClosestPointToEyeTarget(pupil2RootPos);
    }

    private const float MAX_EYE_DISTANCE = 0.07f;
    private Vector3 GetClosestPointToEyeTarget(Vector3 localRootPos) {
        Vector3 worldSpacePos = head.TransformPoint(localRootPos);
        Vector3 targetPos = eyeTarget.position;
        Vector3 diff = targetPos - worldSpacePos;
        if(diff.magnitude < MAX_EYE_DISTANCE) {
            return head.InverseTransformPoint(targetPos);
        } else {
            return localRootPos + (diff.normalized * MAX_EYE_DISTANCE);
        }
    }

    public void SetThrowMode(bool isThrow) {
        headRenderer.sprite = isThrow ? headAlarmSprite : headSprite;
        eyesRenderer.sprite = isThrow ? eyesAlarmSprite : eyesSprite;
    }
}
