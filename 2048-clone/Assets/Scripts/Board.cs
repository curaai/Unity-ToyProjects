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
    }

    public void Shift(Vector2Int direction)
    {
        if (direction.magnitude != 1 || direction.x == direction.y)
            return;

        Debug.Log("Input: " + direction.ToString());
    }
}