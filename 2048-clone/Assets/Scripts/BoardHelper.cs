using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardHelper
{
    private int[,] values;
    private Cell[,] cells;

    private List<Vector2Int> indices;

    public BoardHelper(int[,] _values, Cell[,] _cells)
    {
        this.values = _values;
        this.cells = _cells;

        var _range = Enumerable.Range(0, Board.size).ToList();
        indices = LinqUtil.Permutation(_range, _range)
            .Select(p => new Vector2Int(p.Item1, p.Item2)).ToList();
    }

    public List<Vector2Int> FindEmptyIndexes()
    {
        return indices.Where(i => values[i.x, i.y] == 0).ToList();
    }
}