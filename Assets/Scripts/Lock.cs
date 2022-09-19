using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private DieSprites dieSprites;
    [SerializeField] private Number lockKey;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        this.spriteRenderer.sprite = dieSprites.sideSprites[(int) lockKey];
    }

    public bool Unlock(Number key)
    {
        bool correctKey = key == lockKey;
        if (correctKey) gameObject.SetActive(false);
        return correctKey;
    }
}
