using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UndoManager
{
    private int[,] old;
    private int score;

    private Board board;

    public UndoManager(Board _board)
    {
        board = _board;
        old = new int[Board.size, Board.size];
    }

    public void Capture(int[,] _value)
    {
        score = board.score.value;
        Array.Copy(_value, old, old.Length);
    }

    public void Perform(int[,] _value, Cell[,] _cell)
    {
        foreach (var x in board.cellList)
            GameObject.Destroy(x.gameObject);
        board.cellList.Clear();

        Array.Copy(old, _value, _value.Length);

        for (int i = 0; i < old.GetLength(0); i++)
        {
            for (int j = 0; j < old.GetLength(1); j++)
            {
                var x = old[i, j];
                if (x != 0)
                    board.CreateCell(new Vector2Int(i, j), x, newCell: false);
            }
        }

        board.score.value = score;
    }
}
