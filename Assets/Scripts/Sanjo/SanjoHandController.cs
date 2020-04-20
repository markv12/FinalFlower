using UnityEngine;

public class SanjoHandController : MonoBehaviour {

    public MonoBehaviour owner;
    public bool belongsToPlayer;
    public float heightAdjustment = 0.0f;
    public float armLength = 1.8f;
	public float throwPower = 300.0f;
	
    public Transform shootPoint;
    public Transform handMainTransform;
    public Transform imageT;

    public GameObject catchObject;

    public GameObject closedClaw;
    public GameObject openClaw;

    public GameObject bulletPrefab;

    private static Camera theCamera;
    public static Camera TheCamera
	{
        get 
		{
            if (theCamera == null) 
			{
                theCamera = CameraShaker.instance.shakeTransform.GetComponent<Camera>();
            }
            return theCamera;
        }
    }

    public static float cameraDistance;

    private void Start()
	{
        if (belongsToPlayer) 
		{
            cameraDistance = -TheCamera.transform.position.z;
        }
    }

    private static readonly Quaternion theIQ = Quaternion.identity;
    private static readonly Quaternion shotRotation = Quaternion.Euler(0, 0, -16);
    private static readonly Quaternion shotRotationReverse = Quaternion.Euler(0, 0, 16);
	
	private float lastThrowTime;

    void Update() 
	{
        if (Time.timeScale > 0) 
		{
            Vector3 aimPosition /*= new Vector3( 0.0f, 1.0f, 0.0f )*/;

            if (belongsToPlayer)
			{
                Vector3 v3 = Input.mousePosition;
                v3.z = cameraDistance;
                aimPosition = TheCamera.ScreenToWorldPoint(v3);
            } 
			else 
			{
				if( HasAnyObject() )
				{
					aimPosition = Player.mainPlayer.transform.position;
				}
				else
				{
					SanjoEnemy enemyScript = owner.GetComponent<SanjoEnemy>();

					ThingToProtect protectObj = enemyScript.GetAI().target;

					aimPosition = protectObj.transform.position;
				}
			}

            Vector3 handPos = GetHandPosition(aimPosition);
            float angleFromPlayer = GetHandAngle(handPos);
            AdjustImageForAngle(angleFromPlayer);
            handMainTransform.rotation = Quaternion.Euler(0, 0, angleFromPlayer);
            handMainTransform.position = handPos;

            if (belongsToPlayer && Input.GetMouseButtonDown(0)) 
			{
                if (HasAnyObject())
				{
                    Throw();
                } 
				//else
				//{
				//	FireGun();
				//}
            }

            imageT.localRotation = Quaternion.Lerp(imageT.localRotation, theIQ, Time.deltaTime * 3);
        }

		if( !belongsToPlayer && HasAnyObject() )
		{
			Throw();
		}
	}

    public void Throw()
	{
        if (HasAnyObject())
		{
			SanjoAbleToCarry carryObj = catchObject.GetComponent<SanjoAbleToCarry>();

			if( carryObj )
			{
				carryObj.OnThrow(shootPoint.right * throwPower); 
			}

			catchObject = null;
            closedClaw.SetActive(false);
            openClaw.SetActive(true);

			lastThrowTime = Time.time;
        }
    }

    public void OnCatch(GameObject anyObject) {
        catchObject = anyObject;
        closedClaw.SetActive(true);
        openClaw.SetActive(false);
    }

    public bool HasAnyObject() {
        return catchObject != null;
    }

 //   public void FireGun()
//	 {
 //       GameObject newBullet = Instantiate(bulletPrefab);
 //       newBullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
 //       newBullet.GetComponent<Bullet>().owner = owner;
 //       AudioManager.Instance.PlayGunSound();
 //   }

    private DirectionState dState = DirectionState.initial;
    private void AdjustImageForAngle(float angleFromPlayer) {
        if (angleFromPlayer > -90 && angleFromPlayer < 90) {
            if (dState != DirectionState.right) {
                imageT.localScale = new Vector3(-1, 1, 1);
                imageT.localPosition = new Vector3(0, -heightAdjustment, 0);
                imageT.localRotation = theIQ;
                dState = DirectionState.right;
            }
        } else {
            if (dState != DirectionState.left) {
                imageT.localScale = new Vector3(-1, -1, 1);
                imageT.localPosition = new Vector3(0, heightAdjustment, 0);
                imageT.localRotation = theIQ;
                dState = DirectionState.left;
            }
        }
    }

    private float GetHandAngle(Vector3 gunPos) 
	{
        Vector3 relPosFromPlayer = gunPos - owner.transform.position;

        return Mathf.Atan2(relPosFromPlayer.y, relPosFromPlayer.x) * 57.2958f;
    }

    private Vector3 GetHandPosition(Vector3 aimPosition)
	{
        Vector3 result;
        Vector3 relPosFromPlayer = aimPosition - owner.transform.position;

        relPosFromPlayer.z = 0;

		if( HasAnyObject() && !belongsToPlayer )
		{
			relPosFromPlayer.x *= -1.0f;

			relPosFromPlayer.y += 5.0f;
		}

        if (relPosFromPlayer.magnitude > armLength) 
		{
            Vector3 normalizedAngle = relPosFromPlayer.normalized * armLength;
            result = owner.transform.position + normalizedAngle;
        }
		else
		{
            result = aimPosition;
        }
        return new Vector3(result.x, result.y, 0);
    }
}
