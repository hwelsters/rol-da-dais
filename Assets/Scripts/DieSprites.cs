using UnityEngine;

[CreateAssetMenu(fileName="Create DieSprite", menuName="Create DieSprite")]
public class DieSprites : ScriptableObject
{
    [SerializeField] public Sprite[] sideSprites = new Sprite[(int) Number.SIDE_COUNT];
}
