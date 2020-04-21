using System.Collections;
using UnityEngine;

public class SanjoEnemy : MonoBehaviour 
{	
    public SanjoHandController hand;
	public SanjoAIController AI;
	public Rigidbody2D mainRigidbody;
	
    private Collider2D mainCollider;

	public SanjoAIController GetAI()
	{
		return AI;
	}

    void Start()
	{
        //attackRoutine = StartCoroutine(AttackRoutine());
		mainCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
	{
		AI.Update();
	}

	public void BulletHit(Vector3 hitAngle, Vector2 hitLocation)
	{
        CameraShaker.instance.HitCameraShake();

		mainRigidbody.AddForceAtPosition( hitAngle * 1200, hitLocation );
    }

    private static readonly WaitForSeconds dieWait = new WaitForSeconds(3f);
    private IEnumerator DieRoutine() 
	{
        yield return dieWait;
		
		Destroy(mainCollider);
        Destroy(gameObject, 5);
    }
	
 //   private Coroutine attackRoutine;
 //   private IEnumerator AttackRoutine()
	//{
 //       while (true) 
	//	{
 //           yield return new WaitForSeconds(2f + Random.Range(-1f, 1.5f));

	//		if( hand )
	//		{
	//			hand.FireGun();
	//		}
 //       }
 //   }
}
