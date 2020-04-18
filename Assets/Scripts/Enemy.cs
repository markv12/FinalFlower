using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public bool moveTowardsPlayer = false;

    public Collider2D mainCollider;
    public Collider2D headCollider;
    public Rigidbody2D mainRigidbody;
    public HandController gun;
    public Transform bodyTransform;

    private bool dead = false;
    public void Kill(Vector3 hitAngle, Vector2 hitLocation) {
        dead = true;

        Destroy(gun);
        AudioManager.instance.PlayGunSound();
        StopCoroutine(attackRoutine);
        mainRigidbody.isKinematic = false;
        Debug.Log(hitAngle);
        mainRigidbody.AddForceAtPosition(hitAngle*550, hitLocation);
        StartCoroutine(DieRoutine());
    }

    private static readonly WaitForSeconds dieWait = new WaitForSeconds(3.5f);
    private IEnumerator DieRoutine() {
        yield return dieWait;
        Destroy(mainCollider);
        Destroy(headCollider);
        Destroy(gameObject, 5);
    }

    private Coroutine attackRoutine;
    void Start() {
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private const float MOVE_SPEED = 5;
    private const float MIN_DISTANCE = 4.5f;
    private const float AGGRO_RANGE = 12f;

    void Update() {
        if (!dead) {
            float distanceFromPlayer = Player.mainPlayer.t.position.x - transform.position.x;
            float absDistance = Mathf.Abs(distanceFromPlayer);
            float theX = (MOVE_SPEED * Time.deltaTime * Mathf.Sign(distanceFromPlayer));
            if (moveTowardsPlayer) {
                if (absDistance > MIN_DISTANCE) {
                    transform.position += new Vector3(theX, 0, 0);
                }
            }
            if (absDistance < AGGRO_RANGE) {
                SetHorizontalInput(theX);
            }
        }
        if (Vector3.Distance(transform.position, Player.mainPlayer.t.position) > 50) {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackRoutine() {
        while (true) {
            yield return new WaitForSeconds(2f + Random.Range(-1f, 1.5f));
            gun.FireGun();
        }
    }

    private DirectionState dState = DirectionState.initial;
    private void SetHorizontalInput(float input) {
        if (input > 0) {
            if (dState != DirectionState.right) {
                bodyTransform.localScale = new Vector3(1, 1, 1);
                dState = DirectionState.right;
            }
        } else if (input < 0) {
            if (dState != DirectionState.left) {
                bodyTransform.localScale = new Vector3(-1, 1, 1);
                dState = DirectionState.left;
            }
        }
    }
}
