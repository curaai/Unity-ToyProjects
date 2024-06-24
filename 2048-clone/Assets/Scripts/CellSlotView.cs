using UnityEngine;

public class CellSlotView : MonoBehaviour
{
    public Vector2Int CellPos;

    public void Init(Vector2Int _CellPos)
    {
        CellPos = _CellPos;
        name = $"{CellPos.x} {CellPos.y}";
    }
}