using UnityEngine;

public class SanjoAbleToCarry : MonoBehaviour 
{
	public GameObject owner;
	private Transform mainTransform;
	private Rigidbody2D mainRigidbody;

	private float lastThrowTime;

	private void Start()
	{
		mainRigidbody = GetComponent<Rigidbody2D>();
		mainTransform = GetComponent<Transform>();
	}

	public void OnThrow( Vector2 direction )
	{
		mainTransform.SetParent( null );
		mainRigidbody.isKinematic = false;
		mainRigidbody.AddForce( direction );

		lastThrowTime = Time.time;
	}

	public void Catch( SanjoHandController _handController )
	{
		mainTransform.SetParent( _handController.handMainTransform );
		_handController.OnCatch( owner );

		mainRigidbody.isKinematic = true;
		mainRigidbody.velocity = Vector2.zero;
		mainRigidbody.angularVelocity = 0;
		mainTransform.localPosition = new Vector3( 0.75f, 0, 0 );
		mainTransform.localRotation = Quaternion.identity;
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.collider.tag.CompareTo( owner.tag ) == 0 )
		{
			return;
		}

		if( ( Time.time - lastThrowTime ) > 0.6f )
		{
			Transform theParent = collision.gameObject.transform.parent;

			if( theParent != null )
			{
				SanjoHandController theHandController = theParent.GetComponentInParent<SanjoHandController>();

				if( theHandController != null )
				{
					Catch( theHandController );
				}
			}
		}
	}
}
