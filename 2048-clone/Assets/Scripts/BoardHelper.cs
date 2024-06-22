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
        size = board.values.GetLength(0);

        var _range = Enumerable.Range(0, size).ToList();
        indices = LinqUtil.Permutation(_range, _range)
            .Select(p => new Vector2Int(p.Item1, p.Item2)).ToList();
    }

    public List<Vector2Int> FindEmptyIndexes()
    {
        return indices.Where(i => board.values[i.x, i.y] == 0).ToList();
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

            var value = board.values[nextpos.x, nextpos.y];
            if (value != 0)
            {
                var mergeable = board.cells[nextpos.x, nextpos.y].mergeable
                    && cell.mergeable;
                if (mergeable && value == cell.value)
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
}