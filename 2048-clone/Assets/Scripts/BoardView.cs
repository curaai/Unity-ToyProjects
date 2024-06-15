using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    private readonly float cellViewSpace = 0.1f;
    private readonly Vector2 cellViewSize = Vector2.one;
    private readonly int row = 4;
    private readonly int col = 4;

    [SerializeField] private GameObject cellBgPrefab;
    [SerializeField] private Vector2 leftTopPos = new(-1.75f, 1.75f);

    public readonly List<GameObject> cellViewList = new();

    private void Start()
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                var pos = leftTopPos
                    + Vector2.right * ((1 + i) * cellViewSpace)
                    + Vector2.right * i * cellViewSize.x
                    + Vector2.down * ((1 + j) * cellViewSpace)
                    + Vector2.down * j * cellViewSize.y;

                var cellView = Instantiate(cellBgPrefab, pos, Quaternion.identity, transform);
                cellViewList.Add(cellView);
            }
    }
}