using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 speed;
    public Transform myT;
    public MonoBehaviour owner;

    void Update () {
        myT.Translate(speed * Time.deltaTime);
        if(Vector3.Distance(myT.position, Player.mainPlayer.transform.position) > 50) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        bool didHit = true;
        if (col.gameObject.CompareTag("Player")) {
            var playerScript = col.gameObject.GetComponentInParent<Player>();
            if (playerScript != owner) {
                playerScript.GetHit();
            }
        } else if(col.gameObject.CompareTag("Enemy")) {
            Enemy enemyScript = col.gameObject.GetComponentInParent<Enemy>();
            if (enemyScript != owner && !(owner is Enemy)) {
                enemyScript.Kill(myT.right, myT.position);
            } else {
                didHit = false;
            }
        }
        if (didHit) {
            AudioManager.Instance.PlayHitSound();
            Destroy(gameObject);
        }
    }
}
