using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Snake snake;

    private HashSet<Vector2Int> snakeCellsCache;

    private void Start()
    {
        snake.moved += Do;
        Invoke("Do", 1f);
    }

    private void Do()
    {
        if (!enabled) return;

        var dir = Think();
        if (!dir.HasValue) return;

        snake.ChangeDirection(dir.Value);
    }

    private Vector2Int? Think()
    {
        var dirs = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        snakeCellsCache = snake.CellPositions().ToHashSet();

        var curPos = Vector2Int.FloorToInt(snake.transform.position);
        var safeDirs = dirs.Where(d => IsSafePosition(d + curPos)).ToList();

        if (safeDirs.Count == 0)
            return null;

        var target = Vector2Int.FloorToInt(field.curFruit.transform.position);

        return safeDirs.OrderBy(d => (target - (d + curPos)).magnitude).First();
    }

    private bool IsSafePosition(Vector2Int p)
    {
        if (p.x == -(field.size.x / 2) || p.x == (field.size.x / 2) ||
            p.y == -(field.size.y / 2) || p.y == (field.size.y / 2))
            return false;

        return !snakeCellsCache.Contains(p);
    }
}