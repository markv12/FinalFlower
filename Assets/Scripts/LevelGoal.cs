using UnityEngine;

public class LevelGoal : MonoBehaviour
{
	private float startTime;
	public void Start()
	{
		startTime = Time.time;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Player playerScript = collision.gameObject.GetComponent<Player>();
		if (playerScript != null)
		{
			if (playerScript.handController.HasThingToProtect())
			{
				AudioManager.Instance.PlayWinSound();
				LevelClearManager.LevelClear(Time.time - startTime);
			}
		}
	}
}
