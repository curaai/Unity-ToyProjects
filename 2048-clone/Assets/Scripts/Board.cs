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
        cell.Init(cpos, 2);
        view.SetPosition(cell);

        cells[cpos.x, cpos.y] = cell;
        values[cpos.x, cpos.y] = cell.value;
        cellList.Add(cell);
    }

    public void Shift(Vector2Int direction)
    {
        if (direction.magnitude != 1 || direction.x == direction.y)
            return;

        cellList.ForEach(c => Move(c, helper.NavigateCell(c, direction)));

        void Move(Cell c, Vector2Int dst)
        {
            cells[c.cellPos.x, c.cellPos.y] = null;
            values[c.cellPos.x, c.cellPos.y] = 0;
            cells[dst.x, dst.y] = c;
            values[dst.x, dst.y] = c.value;
            c.cellPos = dst;
            view.SetPosition(c);
        }
    }
}