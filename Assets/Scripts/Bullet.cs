using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 speed;
    public Transform myT;

    void Update () {
        myT.Translate(speed * Time.deltaTime);
        if(Vector3.Distance(myT.position, Player.mainPlayer.transform.position) > 50) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            //col.gameObject.GetComponentInParent<Player>().Health -= 1;
        } else if(col.gameObject.CompareTag("Enemy")) {
            col.gameObject.GetComponentInParent<Enemy>().Health -= 1;
        }
        AudioManager.instance.PlayGunSound();
        Destroy(gameObject);
    }
}
