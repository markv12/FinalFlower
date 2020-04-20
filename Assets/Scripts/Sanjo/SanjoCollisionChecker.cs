using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanjoCollisionChecker : MonoBehaviour
{
	public string tagName1;
	public string tagName2;
	
	public bool isHit() { return hit; }

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

	private bool checkHash( Collider2D collision )
	{
		return ( collision.tag.CompareTo( tagName1) == 0 || collision.tag.CompareTo(tagName2) == 0 );
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		enter = checkHash( collision );
	}
 
	private void OnTriggerStay2D( Collider2D collision )
	{
		stay = checkHash( collision );
	}
     
	private void OnTriggerExit2D( Collider2D collision )
	{
		exit = checkHash( collision );
	}
}
