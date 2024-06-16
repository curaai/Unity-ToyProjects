using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
    private bool movingNow;

    void Start()
    {
        helper = new BoardHelper(values, cells);
        input.onInput += Shift;

        view.Init(size);

        CreateCell(Vector2Int.zero);
        CreateCell(new Vector2Int(0, 1));
        CreateCell(new Vector2Int(0, 2));

        // Test
        var last = cellList.Last();
        last.Set(last.cellPos, 4);
        // GenerateRandomCell();
        // GenerateRandomCell();
    }

    public void GenerateRandomCell()
    {
        // TODO: 빈 셀이 없을 때 게임종료 조건 추가 
        CreateCell(helper.FindEmptyIndexes().PickRandom());
    }

    public void CreateCell(Vector2Int pos)
    {
        var cell = Instantiate(cellPrefab, view.transform);
        cell.Set(pos, 2);
        view.SetPosition(cell);

        cells[pos.x, pos.y] = cell;
        values[pos.x, pos.y] = cell.value;
        cellList.Add(cell);
    }

    public void Shift(Vector2Int direction)
    {
        // TODO: 현재 움직일수 없는 방향으로 입력됐을 때 무시
        if (direction.magnitude != 1 || direction.x == direction.y)
            return;
        if (movingNow)
            return;

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

            ClearCell(c.cellPos);

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

    private void ClearCell(Vector2Int pos)
    {
        cells[pos.x, pos.y] = null;
        values[pos.x, pos.y] = 0;
    }

    private void Set(Cell c, Vector2Int pos)
    {
        cells[pos.x, pos.y] = c;
        values[pos.x, pos.y] = c.value;
        c.cellPos = pos;
    }
}