using System.Collections;
using UnityEngine;

public class SanjoEnemy : MonoBehaviour 
{	
    public Collider2D mainCollider;
    public Collider2D headCollider;
    public Rigidbody2D mainRigidbody;
    public HandController gun;
    public Transform bodyTransform;
	public SanjoAIController AI;
		
    void Start()
	{
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    void Update()
	{
		AI.Update();
	}

	public void Kill(Vector3 hitAngle, Vector2 hitLocation)
	{
        CameraShaker.instance.HitCameraShake();

		if( gun )
		{
			Destroy( gun );
		}

        StopCoroutine(attackRoutine);
        mainRigidbody.isKinematic = false;
        mainRigidbody.AddForceAtPosition( hitAngle * 600, hitLocation );
        StartCoroutine(DieRoutine());
    }

    private static readonly WaitForSeconds dieWait = new WaitForSeconds(3f);
    private IEnumerator DieRoutine() 
	{
        yield return dieWait;
        Destroy(mainCollider);
        Destroy(headCollider);
        Destroy(gameObject, 5);
    }
	
    private Coroutine attackRoutine;
    private IEnumerator AttackRoutine()
	{
        while (true) 
		{
            yield return new WaitForSeconds(2f + Random.Range(-1f, 1.5f));

			if( gun )
			{
				gun.FireGun();
			}
        }
    }
}
