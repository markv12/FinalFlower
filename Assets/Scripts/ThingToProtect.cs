using UnityEngine;

public class ThingToProtect : MonoBehaviour {
    public Transform mainTransform;
    public Rigidbody2D mainRigidbody;

    private float lastThrowTime;

    public void Throw(Vector2 direction) {
        AudioManager.Instance.EnableWarningMusic(true);
        mainTransform.SetParent(null);
        mainRigidbody.isKinematic = false;
        mainRigidbody.AddForce(direction*450);

        lastThrowTime = Time.time;
    }

    public void Catch(HandController _handController) {
        AudioManager.Instance.EnableWarningMusic(false);
        mainTransform.SetParent(_handController.handMainTransform);
        _handController.thingToProtect = this;

        mainRigidbody.isKinematic = true;
        mainRigidbody.velocity = Vector2.zero;
        mainRigidbody.angularVelocity = 0;
        mainTransform.localPosition = new Vector3(0.6f, 0, 0);
        mainTransform.localRotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if ((Time.time - lastThrowTime) > 1f) {
            Transform theParent = collision.gameObject.transform.parent;
            if (theParent != null) {
                HandController theHandController = theParent.GetComponent<HandController>();
                if (theHandController != null) {
                    Catch(theHandController);
                }
            }
        }
        if (collision.gameObject.CompareTag("Floor")) {
            Vector2 currPos = mainTransform.position;
            float highestY = GetHighestY(collision.gameObject);
            if(currPos.y > highestY) {
                GameOverManager.GameOver();
            }
            //for (int i = 0; i < collision.contactCount; i++) {
            //    ContactPoint2D cont = collision.GetContact(i);
            //    Vector2 contactLocation = cont.point;
            //    if (contactLocation.y < currPos.y) {
            //        //GameOverManager.GameOver();
            //    }
            //}
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
        if (mainTransform.position.y < -15) {
            GameOverManager.GameOver();
        }
    }
}
