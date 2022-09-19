using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTile : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.floor.SetTile(new Vector3Int((int) transform.position.x - 1, (int) transform.position.y - 1, 0), null);
            gameObject.SetActive(false);
        }
    }
}
