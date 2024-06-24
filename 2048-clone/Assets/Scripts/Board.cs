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

    public List<Cell> cellList = new();
    private bool movingNow;
    private List<Vector2Int> availableInput;
    public Action gameovered;

    public Score score = new();
    private BoardHelper helper;
    private UndoManager undo;

    void Start()
    {
        undo = new UndoManager(this);
        helper = new BoardHelper(this);

        input.onInput += Shift;
        input.onUndo += Undo;

        view.Init(size);

        GenerateRandomCell();
        GenerateRandomCell();
    }

    public void GenerateRandomCell()
    {
        CreateCell(helper.FindEmptyIndexes().PickRandom());
        Refresh();
        undo.Capture();
    }

    public void CreateCell(Vector2Int pos, int value = 2, bool newCell = true)
    {
        var cell = Instantiate(cellPrefab, view.transform);
        cell.Set(pos, value, newCell);
        view.SetPosition(cell);

        cellList.Add(cell);
    }

    public void Shift(Vector2Int direction)
    {
        if (!availableInput.Contains(direction)) return;
        if (direction.magnitude != 1 || direction.x == direction.y) return;
        if (movingNow) return;

        YieldCollection yields = new();
        List<Cell> mergeCells = new();

        StartCoroutine(f());
        IEnumerator f()
        {
            movingNow = true;

            cellList.ForEach(c => c.mergeable = true);
            helper.OrderCellsByDirection(direction).ForEach(Move);
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

            var dstCell = Get(dst);
            if (dstCell)
                Merge(c, dstCell);
            else
                c.cellPos = dst;

            StartCoroutine(yields.CountCoroutine(c.Move(view.GetWorldPos(dst))));
        }

        void Merge(Cell from, Cell to)
        {
            from.transform.position += Vector3.back;
            to.Merged();
            to.mergeable = false;
            score.value += to.value;
            cellList.Remove(from);
            mergeCells.Add(from);
        }
    }

    private void Refresh()
    {
        availableInput = helper.AvailableShifts();
        if (availableInput.Count == 0)
            gameovered?.Invoke();
    }

    private void Undo()
    {
        if (movingNow) return;

        undo.Perform();
        Refresh();
    }

    public Cell Get(Vector2Int v)
    {
        return cellList.FirstOrDefault(c => c.cellPos == v);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}