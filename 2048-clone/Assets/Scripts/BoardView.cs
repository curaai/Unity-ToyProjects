using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    private readonly float slotInterval = 0.1f;
    private readonly Vector2 slotSize = Vector2.one;

    [SerializeField] private CellSlotView slotPrefab;
    [SerializeField] private Vector2 leftBotPos = new(-1.75f, -1.75f);

    public CellSlotView[,] slots;

    public void Init(int size)
    {
        slots = new CellSlotView[size, size];

        var _range = Enumerable.Range(0, size).ToList();
        foreach ((var i, var j) in LinqUtil.Permutation(_range, _range))
        {
            var worldPos = leftBotPos
                + Vector2.right * ((1 + i) * slotInterval)
                + Vector2.right * i * slotSize.x
                + Vector2.up * ((1 + j) * slotInterval)
                + Vector2.up * j * slotSize.y;

            var slot = Instantiate(slotPrefab, worldPos, Quaternion.identity, transform);
            slot.Init(new Vector2Int(i, j));
            slots[i, j] = slot;
        }
    }

    public void SetPosition(Cell cell)
    {
        var slot = slots[cell.cellPos.x, cell.cellPos.y];
        cell.transform.position = slot.transform.position;
    }

    public Vector3 GetWorldPos(Vector2Int cellPos)
    {
        return slots[cellPos.x, cellPos.y].transform.position;
    }
}