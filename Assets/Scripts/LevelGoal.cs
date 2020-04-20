using UnityEngine;

public class LevelGoal : MonoBehaviour {
	public Transform flowerPosition;
	private float startTime;
	public void Start() {
		startTime = Time.time;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		ThingToProtect thingToProtect = collision.gameObject.GetComponent<ThingToProtect>();
		if (thingToProtect != null) {
			if (Player.mainPlayer.handController.HasThingToProtect()) {
				AudioManager.Instance.PlayWinSound();
				LevelClearManager.LevelClear(Time.time - startTime);
				thingToProtect.transform.SetParent(flowerPosition, false);
				thingToProtect.transform.localPosition = new Vector3(0, 0, 0);
				thingToProtect.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
			}
		}
	}
}
