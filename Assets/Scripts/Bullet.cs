using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Vector3 speed;
	public Transform myT;
	public MonoBehaviour owner;

	void Update()
	{
		myT.Translate(speed * Time.deltaTime);
		Vector3 pos = myT.position;
		if (Vector3.Distance(pos, Player.mainPlayer.transform.position) > 50)
		{
			Destroy(gameObject);
		}
		Vector3 viewportPosition = HandController.TheCamera.WorldToViewportPoint(pos);
		if (Mathf.Abs(viewportPosition.x) > 1.05f || Mathf.Abs(viewportPosition.y) > 1.05f)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		bool didHit = true;
		if (col.gameObject.CompareTag("Player"))
		{
			var playerScript = col.gameObject.GetComponentInParent<Player>();
			if (playerScript != owner)
			{
				AudioManager.Instance.PlayHitPlayerSound();
				playerScript.GetHit();
			}
		}
		else if (col.gameObject.CompareTag("Enemy"))
		{
			Enemy enemyScript = col.gameObject.GetComponentInParent<Enemy>();
			if (enemyScript != null && enemyScript != owner && !(owner is Enemy))
			{
				AudioManager.Instance.PlayHitEnemySound();
				enemyScript.Kill(myT.right, myT.position);
			}
			else
			{
				didHit = false;
			}
		}
		else
		{
			AudioManager.Instance.PlayHitWallSound();
		}
		if (didHit)
		{
			Destroy(gameObject);
		}
	}
}
