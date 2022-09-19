using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private const float DELAY = 0.2f;

    [SerializeField] private Vector2 newPosition;

    private static bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTeleport) 
        {
            canTeleport = false;
            StartCoroutine(TeleportAfterDelay(other));
            StartCoroutine(ResetTeleport());
        }
    }

    private IEnumerator TeleportAfterDelay(Collider2D other)
    {
        other.GetComponent<Player>().SetCanMove(false);
        yield return new WaitForSeconds(DELAY);
        other.GetComponent<Player>().StopAllRoutines();
        other.transform.position = newPosition;
        other.GetComponent<Player>().SetCanMove(true);
    }

    private IEnumerator ResetTeleport()
    {
        yield return new WaitForSeconds(DELAY * 2);
        canTeleport = true;
    }
}
