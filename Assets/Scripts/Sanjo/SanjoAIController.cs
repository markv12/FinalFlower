using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanjoAIController : MonoBehaviour
{
	public bool isWalker = true;
	public bool isJumper = true;

	public GameObject owner;
	public ThingToProtect target;

	public float movePower = 80.0f;
	public float jumpPowerX = 1.0f;
	public float jumpPowerY = 8.0f;
	public float minDistance = 0.0f;
	public float detectRange = 12.0f;
	public float lostRange = 18.0f;

	public SanjoCollisionChecker floorChecker = null;
	public SanjoCollisionChecker pitfallChecker = null;
	public SanjoCollisionChecker wallChecker = null;
	public SanjoCollisionChecker attackCollision = null;

	// Start is called before the first frame update
	void Start()
	{
		rigidBody = owner.GetComponent<Rigidbody2D>();
	}

	private Rigidbody2D rigidBody;

	private bool isDetected = false;
	private bool isLanding = false;

	private bool dead = false;

	// Update is called once per frame
	public void Update()
	{
		Transform targetTrans;

		if( !target )
		{
			targetTrans = Player.mainPlayer.transform;
		}
		else
		{
			targetTrans = target.transform;
		}

		if( Vector3.Distance( transform.position, target.transform.position ) > 50 )
		{
			Destroy( gameObject );
		}

		if( dead )
		{
			return;
		}

		float distanceFromTarget = target.transform.position.x - transform.position.x;

		UpdateMove( distanceFromTarget );

		UpdateDetection( distanceFromTarget );

		UpdateContactAttack();

		wallChecker.SetOffsetX( Mathf.Sign( distanceFromTarget ) );
		pitfallChecker.SetOffsetX( Mathf.Sign( distanceFromTarget ) );
	}

	private void UpdateMove( float distance )
	{
		Vector2 move = rigidBody.velocity;

		if( isDetected )
		{
			if( floorChecker.isHit() )
			{
				bool obstacleAhead = wallChecker.isHit() || !pitfallChecker.isHit();

				if( isWalker && !obstacleAhead )
				{
					float velocityX = ( movePower * Time.deltaTime * Mathf.Sign( distance ) );

					if( Mathf.Abs( distance ) > minDistance )
					{
						move.x = velocityX;
					}
				}

				if( isJumper )
				{
					float heightFromTarget = target.transform.position.y - transform.position.y;

					bool doJump = obstacleAhead | ( heightFromTarget > 4.0f );

					if( doJump )
					{
						move.y = jumpPowerY * 0.5f;

						if( obstacleAhead )
						{
							move.x = jumpPowerX * Mathf.Sign( distance );
							move.y = jumpPowerY;
						}

						if( !pitfallChecker.isHit() )
						{
							move.x *= 2.0f;
						}
					}
					else
					{

					}
				}
			}
		}

		rigidBody.velocity = move;
	}

	private void UpdateDetection( float distance )
	{
		float absDistance = Mathf.Abs( distance );

		if( isDetected )
		{
			if( absDistance > lostRange )
			{
				isDetected = false;
			}
		}
		else if( absDistance < detectRange )
		{
			isDetected = true;
		}
	}

	private void UpdateContactAttack()
	{
		if( attackCollision.isHit() )
		{
			Player.mainPlayer.GetHit();
		}
	}
}