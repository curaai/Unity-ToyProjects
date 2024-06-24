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

    public void Capture()
    {
        score = board.score.value;
        Array.Clear(old, 0, old.Length);
        board.cellList.ForEach(c => old[c.cellPos.x, c.cellPos.y] = c.value);
    }

    public void Perform()
    {
        foreach (var x in board.cellList)
            GameObject.Destroy(x.gameObject);

        board.cellList.Clear();

        ValidPosList().ForEach(p =>
            board.CreateCell(new(p.i, p.j), old[p.i, p.j], newCell: false));

        List<(int i, int j)> ValidPosList()
        {
            var res = new List<(int i, int j)>();

            for (int i = 0; i < old.GetLength(0); i++)
                for (int j = 0; j < old.GetLength(1); j++)
                    if (old[i, j] != 0)
                        res.Add((i, j));
            return res;
        }
    }
}
