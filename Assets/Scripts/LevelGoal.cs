using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        Player playerScript = collision.gameObject.GetComponent<Player>();
        if(playerScript != null) {
            if (playerScript.handController.HasThingToProtect()) {
                LevelClearManager.LevelClear();
            }
        }
    }
}
