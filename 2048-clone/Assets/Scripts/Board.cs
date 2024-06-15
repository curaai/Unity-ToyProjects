using UnityEngine;

public class Board : MonoBehaviour
{
    public static readonly int row = 4;
    public static readonly int col = 4;

    [SerializeField] private BoardView view;
    [SerializeField] private Cell cellPrefab;

    void Start()
    {
        view.Init();

        var a = Instantiate(cellPrefab, transform);
        a.Init(Vector2Int.one, 2);
        view.SetPosition(a);
        var b = Instantiate(cellPrefab, transform);
        b.Init(Vector2Int.one * 2, 4);
        view.SetPosition(b);
    }
}