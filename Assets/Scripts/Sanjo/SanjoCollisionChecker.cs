using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanjoCollisionChecker : MonoBehaviour
{
	public string tagName1;
	public string tagName2;
	
	public bool isHit() { return hit; }

	private int tagHash1 = 0;
	private int tagHash2 = 0;

	private BoxCollider2D box;

	private Vector2 offset;

	public void SetOffset( Vector2 offset )
	{
		box.offset = offset;
	}

	public void SetOffsetX( float offsetX )
	{
		box.offset = new Vector2(offsetX, box.offset.y);
	}
	private bool hit = false;
	private bool enter = false;
	private bool stay = false;
	private bool exit = false;
	
	private void Start()
	{
		tagHash1 = tagName1.GetHashCode();
		tagHash2 = tagName2.GetHashCode();

		box = GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
		if( enter || stay ) 
		{
			hit = true; 
		}
		else if( exit )
		{
			hit = false; 
		}

		enter = false;
		stay = false;
		exit = false;
	}

	private bool checkHash( int hash )
	{
		return ( hash == tagHash1 || hash == tagHash2 );
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		enter = checkHash( collision.tag.GetHashCode() );
	}
 
	private void OnTriggerStay2D( Collider2D collision )
	{
		stay = checkHash( collision.tag.GetHashCode() );
	}
     
	private void OnTriggerExit2D( Collider2D collision )
	{
		exit = checkHash( collision.tag.GetHashCode() );
	}
}
