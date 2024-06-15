using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellView view;

    public Vector2Int cellPos;
    public int value;

    public void Init(Vector2Int _cellPos, int _value)
    {
        cellPos = _cellPos;
        value = _value;
        view.Set(this);
    }
}