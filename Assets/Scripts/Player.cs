using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{   
    [SerializeField] private DieSprites dieSprites;
    [SerializeField] private Number currentSide;

    private const float MOVE_TIME = 0.075f; 
    private const float INVERSE_MOVE_TIME = 1f / MOVE_TIME;
    private const float TURN_DELAY = 0f;

    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private bool canMove = true;

    public void StopAllRoutines()
    {
        canMove = true;
        StopAllCoroutines();
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void Start()
    {
        InitializeFields();
    }

    private void InitializeFields()
    {
        this.rb2D = GetComponent<Rigidbody2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandleInput();
        ChangeSprite();
    }

    private void HandleInput() 
    {
        int xDir = (int) Input.GetAxisRaw("Horizontal");
        int yDir = (int) Input.GetAxisRaw("Vertical");

        if (xDir != 0) yDir = 0;

        Vector2 finalPosition = (Vector2) transform.position + new Vector2(xDir, yDir);
        if (canMove && (xDir != 0 || yDir != 0)) AttemptMove(finalPosition);
    }

    private void ChangeSides(int xDir, int yDir) 
    {
        this.currentSide = (Number)((((int) this.currentSide) + 1) % (int) Number.SIDE_COUNT);
        // if (xDir == 1)  currentSide = dieDirections[currentSide][(int) Direction.LEFT];
        // if (xDir == -1) currentSide = dieDirections[currentSide][(int) Direction.RIGHT];
        // if (yDir == 1)  currentSide = dieDirections[currentSide][(int) Direction.DOWN];
        // if (yDir == -1) currentSide = dieDirections[currentSide][(int) Direction.UP];
    }

    private void ChangeSprite() { if (dieSprites.sideSprites.Length != 0) spriteRenderer.sprite = dieSprites.sideSprites[(int) currentSide]; }

    private void AttemptMove(Vector2 end)
    {
        bool spacePassable = false;

        // Perform multiple checks to see if player is allowed to move
        spacePassable |= SpaceIsOpen(end);
        spacePassable |= CheckLock(end);
        spacePassable &= ThereIsLand(end);

        if (!spacePassable) return;

        canMove = false;
        ChangeSides((int)(transform.position.x - end.x), (int)(transform.position.y - end.y));
        StartCoroutine(SmoothMovement(end));
    }
    
    private IEnumerator SmoothMovement(Vector2 end)
    {
        float sqrRemainingDistance = ((Vector2) transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, end, INVERSE_MOVE_TIME * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = ((Vector2) transform.position - end).sqrMagnitude;
            yield return null;
        }

        yield return new WaitForSeconds(TURN_DELAY);
        canMove = true;
    }

    private bool CheckLock(Vector2 end)
    {
        return ComponentCast<Lock>(end) == null || ComponentCast<Lock>(end).Unlock(this.currentSide);
    }

    private bool SpaceIsOpen(Vector2 end)
    {
        return ComponentCast<Wall>(end) == null;
    }

    private bool ThereIsLand(Vector2 end)
    {
        return GameManager.floor.GetTile(new Vector3Int((int) end.x - 1, (int) end.y - 1, 0)) != null;
    }

    private T ComponentCast<T>(Vector2 end)
        where T : Component
    {
        RaycastHit2D hit = Linecast(end);

        if (hit.collider != null) return hit.collider.GetComponent<T>();
        return null;
    }

    private RaycastHit2D Linecast (Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, end, LayerMask.GetMask("BlockingLayer"));
        return hit;
    }
    
}
