using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    public Transform wheel;
    public Transform head;

    public Vector3 headLowPosition;
    public Vector3 headTopPosition;

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

}
