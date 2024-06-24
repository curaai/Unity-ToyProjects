using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardHelper
{
    private int size;
    private Board board;

    private List<Vector2Int> indices;

    public BoardHelper(Board _board)
    {
        this.board = _board;
        size = Board.size;

        var _range = Enumerable.Range(0, size).ToList();
        indices = LinqUtil.Permutation(_range, _range)
            .Select(p => new Vector2Int(p.Item1, p.Item2)).ToList();
    }

    public List<Vector2Int> FindEmptyIndexes()
    {
        return indices.Where(i => board.Get(i) == null).ToList();
    }

    public Vector2Int NavigateCell(Cell cell, Vector2Int direction)
    {
        var nextpos = cell.cellPos;
        while (true)
        {
            nextpos += direction;
            var curpos = nextpos - direction;
            if (OutOfBoard(nextpos))
                return curpos;

            var value = GetValue(nextpos);
            if (value.HasValue && value.Value != 0)
            {
                var mergeable = board.Get(nextpos).mergeable
                    && cell.mergeable;
                if (mergeable && value.Value == cell.value)
                    return nextpos;
                else
                    return curpos;
            }
        }

        bool OutOfBoard(Vector2Int cpos)
        {
            if (cpos.x < 0 || cpos.y < 0) return true;
            if (size <= cpos.x || size <= cpos.y) return true;
            return false;
        }
    }

    public List<Vector2Int> AvailableShifts()
    {
        return board.cellList.SelectMany(AvailableShift).ToHashSet().ToList();

        List<Vector2Int> AvailableShift(Cell c)
        {
            List<Vector2Int> shifts = new() {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right,
            };

            return shifts.Where(s =>
            {
                var x = GetValue(c.cellPos + s);
                return !x.HasValue || x.Equals(c.value);
            }).ToList();
        }
    }

    private int? GetValue(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0)
            return null;
        if (size <= pos.x || size <= pos.y)
            return null;
        return board.Get(pos)?.value;
    }

    public List<Cell> OrderCellsByDirection(Vector2Int direction)
    {
        List<Cell> ordered;
        if (direction.x != 0)
            ordered = board.cellList.OrderBy(c => c.cellPos.x).ToList();
        else
            ordered = board.cellList.OrderBy(c => c.cellPos.y).ToList();

        if (direction.x == 1 || direction.y == 1)
            ordered.Reverse();

        return ordered;
    }

    public void Test_FillImpossible()
    {
        var values = new List<int>()
        {
            4,2,4,2,
            2,4,2,4,
            4,2,4,2,
            2,4,2,4,
        };
        indices.Zip(values, (p, v) =>
        {
            board.CreateCell(p, v, true);
            return 0;
        });
    }
}