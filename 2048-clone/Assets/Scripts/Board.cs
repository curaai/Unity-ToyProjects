using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static readonly int size = 4;

    [SerializeField] private BoardView view;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private InputManager input;

    private int[,] values = new int[size, size];
    private Cell[,] cells = new Cell[size, size];
    private List<Cell> cellList = new();
    private BoardHelper helper;

    void Start()
    {
        helper = new BoardHelper(values, cells);
        input.onInput += Shift;

        view.Init(size);

        GenerateRandomCell();
        GenerateRandomCell();
    }

    public void GenerateRandomCell()
    {
        // TODO: 빈 셀이 없을 때 게임종료 조건 추가 
        var cpos = helper.FindEmptyIndexes().PickRandom();
        var cell = Instantiate(cellPrefab, view.transform);
        cell.Set(cpos, 2);
        view.SetPosition(cell);

        cells[cpos.x, cpos.y] = cell;
        values[cpos.x, cpos.y] = cell.value;
        cellList.Add(cell);
    }

    public void Shift(Vector2Int direction)
    {
        if (direction.magnitude != 1 || direction.x == direction.y)
            return;

        OrderCellsByDirection(direction).ForEach(Move);
        GenerateRandomCell();

        void Move(Cell c)
        {
            var dst = helper.NavigateCell(c, direction);

            // Clear
            cells[c.cellPos.x, c.cellPos.y] = null;
            values[c.cellPos.x, c.cellPos.y] = 0;

            if (cells[dst.x, dst.y] != null)
            {
                Merge(c, cells[dst.x, dst.y]);
            }
            else
            {
                cells[dst.x, dst.y] = c;
                values[dst.x, dst.y] = c.value;
                c.cellPos = dst;
                view.SetPosition(c);
            }
        }

        void Merge(Cell from, Cell to)
        {
            cellList.Remove(from);
            to.Set(to.cellPos, to.value * 2);

            Destroy(from.gameObject);
        }

        List<Cell> OrderCellsByDirection(Vector2Int direction)
        {
            List<Cell> ordered;
            if (direction.x != 0)
                ordered = cellList.OrderBy(c => c.cellPos.x).ToList();
            else
                ordered = cellList.OrderBy(c => c.cellPos.y).ToList();

            if (direction.x == 1 || direction.y == 1)
                ordered.Reverse();

            return ordered;
        }
    }
}