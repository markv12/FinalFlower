using UnityEngine;

public class ThingToProtect : MonoBehaviour {
    public Transform mainTransform;
    public Rigidbody2D mainRigidbody;

    private float lastThrowTime;

    public void Throw(Vector2 direction) {
        AudioManager.Instance.EnableWarningMusic(true);
        AudioManager.Instance.PlayCapsuleThrow();
        mainTransform.SetParent(null);
        mainRigidbody.isKinematic = false;
        mainRigidbody.AddForce(direction*450);

        lastThrowTime = Time.time;
    }

    public void Catch(HandController _handController) {
        AudioManager.Instance.EnableWarningMusic(false);
        AudioManager.Instance.PlayCapsuleCatch();
        mainTransform.SetParent(_handController.handMainTransform);
        _handController.Catch(this);

        mainRigidbody.isKinematic = true;
        mainRigidbody.velocity = Vector2.zero;
        mainRigidbody.angularVelocity = 0;
        mainTransform.localPosition = new Vector3(0.75f, 0, 0);
        mainTransform.localRotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if ((Time.time - lastThrowTime) > 0.6f) {
            Transform theParent = collision.gameObject.transform.parent;
            if (theParent != null) {
                HandController theHandController = theParent.GetComponentInParent<HandController>();
                if (theHandController != null) {
                    Catch(theHandController);
                }
            }
        }

		bool isBreak = false;

		isBreak = collision.gameObject.CompareTag( "Enemy" );

        if (collision.gameObject.CompareTag("Floor") ) {
            Vector2 currPos = mainTransform.position;
            float highestY = GetHighestY(collision.gameObject);
            
			if(currPos.y > highestY)
			{
				isBreak |= (currPos.y > highestY);
            }
        }

		if( isBreak )
		{
			AudioManager.Instance.PlayCapsuleBreak();
            GameOverManager.GameOver();	
		}
    }

    private float GetHighestY(GameObject rootObject) {
        SpriteRenderer[] allRenderers = rootObject.GetComponentsInChildren<SpriteRenderer>();
        float result = float.MinValue;
        for (int i = 0; i < allRenderers.Length; i++) {
            Bounds theBounds = allRenderers[i].bounds;
            float highY = (theBounds.center + theBounds.extents).y;
            if(highY > result) {
                result = highY;
            }
        }
        return result;
    }

    private void Update() {
        if (mainTransform.position.y < -15 && Time.timeScale > 0) {
            GameOverManager.GameOver();
        }
    }
}
