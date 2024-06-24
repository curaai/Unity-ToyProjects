using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static readonly int size = 4;

    [SerializeField] private BoardView view;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private InputManager input;

    public int[,] values = new int[size, size];
    public Cell[,] cells = new Cell[size, size];
    public List<Cell> cellList = new();
    private BoardHelper helper;
    private UndoManager undo;
    private bool movingNow;
    private List<Vector2Int> availableInput;

    void Start()
    {
        undo = new UndoManager(this);
        helper = new BoardHelper(this);
        input.onInput += Shift;
        input.onUndo += Undo;

        view.Init(size);

        GenerateRandomCell();
        GenerateRandomCell();
        undo.Capture(values);
    }

    public void GenerateRandomCell()
    {
        CreateCell(helper.FindEmptyIndexes().PickRandom());

        // TODO: 빈 셀이 없을 때 게임종료 조건 추가 
        availableInput = helper.AvailableShifts();
        if (availableInput.Count == 0)
            throw new NotImplementedException("TODO: Game OVER");
    }

    public void CreateCell(Vector2Int pos, int value = 2, bool newCell = true)
    {
        var cell = Instantiate(cellPrefab, view.transform);
        cell.Set(pos, value, newCell);
        view.SetPosition(cell);

        cells[pos.x, pos.y] = cell;
        values[pos.x, pos.y] = cell.value;
        cellList.Add(cell);
    }

    public void Shift(Vector2Int direction)
    {
        if (!availableInput.Contains(direction)) return;
        if (direction.magnitude != 1 || direction.x == direction.y) return;
        if (movingNow) return;

        undo.Capture(values);

        YieldCollection yields = new();
        List<Cell> mergeCells = new();

        StartCoroutine(f());
        IEnumerator f()
        {
            movingNow = true;

            cellList.ForEach(c => c.mergeable = true);
            OrderCellsByDirection(direction).ForEach(Move);
            yield return yields;

            foreach (var x in mergeCells)
                Destroy(x.gameObject);

            yield return null;

            GenerateRandomCell();

            movingNow = false;
        }

        void Move(Cell c)
        {
            var dst = helper.NavigateCell(c, direction);
            if (c.cellPos == dst)
                return;

            cells[c.cellPos.x, c.cellPos.y] = null;
            values[c.cellPos.x, c.cellPos.y] = 0;

            if (cells[dst.x, dst.y] != null)
                Merge(c, cells[dst.x, dst.y]);
            else
                Set(c, dst);

            StartCoroutine(yields.CountCoroutine(c.Move(view.GetWorldPos(dst))));
        }

        void Merge(Cell from, Cell to)
        {
            from.transform.position += Vector3.back;
            to.Merged();
            to.mergeable = false;
            values[to.cellPos.x, to.cellPos.y] *= 2;
            cellList.Remove(from);
            mergeCells.Add(from);
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

    private void Set(Cell c, Vector2Int pos)
    {
        cells[pos.x, pos.y] = c;
        values[pos.x, pos.y] = c.value;
        c.cellPos = pos;
    }

    private void Undo()
    {
        if (movingNow) return;

        undo.Perform(values, cells);
    }
}