using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    private readonly float slotInterval = 0.1f;
    private readonly Vector2 slotSize = Vector2.one;

    [SerializeField] private CellSlotView slotPrefab;
    [SerializeField] private Vector2 leftTopPos = new(-1.75f, 1.75f);

    public readonly List<CellSlotView> slots = new();

    public void Init()
    {
        for (int i = 0; i < Board.row; i++)
            for (int j = 0; j < Board.col; j++)
            {
                var pos = leftTopPos
                    + Vector2.right * ((1 + i) * slotInterval)
                    + Vector2.right * i * slotSize.x
                    + Vector2.down * ((1 + j) * slotInterval)
                    + Vector2.down * j * slotSize.y;

                var slot = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
                slot.Init(new Vector2Int(i, j));
                slots.Add(slot);
            }
    }

    public void SetPosition(Cell cell)
    {
        var pos = slots.Find(s => s.CellPos == cell.cellPos).transform.position;
        cell.transform.position = pos;
    }
}