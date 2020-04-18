using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Collider2D mainCollider;
    public Collider2D headCollider;
    public HandController gun;
    public Transform bodyTransform;

    [SerializeField]
    private float health = 2;
    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health <= 0) {
                Destroy(mainCollider);
                Destroy(headCollider);
                Destroy(gun);
                AudioManager.instance.PlayGunSound();
                StopCoroutine(attackRoutine);
                Destroy(gameObject, 5);
                //Player.mainPlayer.Score++;
            }
        }
    }

    private Coroutine attackRoutine;
    void Start() {
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private const float MOVE_SPEED = 5;
    private const float MIN_DISTANCE = 4.5f;

    void Update() {
        if (health >= 0) {
            float distanceFromPlayer = Player.mainPlayer.t.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) > MIN_DISTANCE) {
                float theX = (MOVE_SPEED * Time.deltaTime * Mathf.Sign(distanceFromPlayer));
                SetHorizontalInput(theX);
                transform.position += new Vector3(theX, 0, 0);
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
