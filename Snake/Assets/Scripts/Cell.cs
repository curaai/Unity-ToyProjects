using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    public Vector2Int pos
    {
        get => Vector2Int.FloorToInt((Vector2)transform.position);
        set => transform.position = (Vector2)value;
    }
}