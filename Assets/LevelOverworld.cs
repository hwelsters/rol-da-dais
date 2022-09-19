using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverworld : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private LevelOverworld[] surroundingObjects = new LevelOverworld[4];
    [SerializeField] private Sprite completedLevelSprite;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private bool playerTouching = false;

    private void OnTriggerEnter2D (Collider2D other) { if (other.CompareTag("Player")) playerTouching = true; }
    private void OnTriggerExit2D (Collider2D other) { if (other.CompareTag("Player")) playerTouching = false; }

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (GameManager.completedLevels.Contains(levelName))  
        {
            foreach (LevelOverworld surroundingObject in surroundingObjects)
            {
                surroundingObject?.gameObject.SetActive(true);
            }
            this.spriteRenderer.sprite = completedLevelSprite;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerTouching) SceneManager.LoadScene(levelName);

        
    }

    private T ComponentCast<T>(Vector2 end)
        where T : Component
    {
        this.boxCollider.enabled = false;
        RaycastHit2D hit = Linecast(end);
        this.boxCollider.enabled = true;

        if (hit.collider != null) return hit.collider.GetComponent<T>();
        return null;
    }

    private RaycastHit2D Linecast (Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, end, LayerMask.GetMask("BlockingLayer"));
        return hit;
    }
}
