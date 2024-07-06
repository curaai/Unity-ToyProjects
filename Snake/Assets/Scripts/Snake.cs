using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Cell cellPrefab;

    public Action<int> collisioned;
    public Action moved;

    private InputManager input;
    private Vector2Int dir;
    public List<Cell> cells = new();
    private Vector2Int tail;
    private RainbowGradient rainbow;

    private void Start()
    {
        rainbow = new();

        input = GetComponent<InputManager>();
        input.onInput += ChangeDirection;
        cells.Add(GetComponentInChildren<Cell>());

        StartCoroutine(_Update());
    }

    private IEnumerator _Update()
    {
        while (speed > 0)
        {
            if (dir == Vector2Int.zero)
            {
                yield return null;
                continue;
            }

            var posList = cells.Select(c => c.pos).ToList();

            transform.position += (Vector3)(Vector2)dir;
            for (int i = 1; i < cells.Count; i++)
                cells[i].pos = posList[i - 1];

            tail = posList.Last();

            yield return new WaitForSeconds(1 / speed);
            moved?.Invoke();
        }
    }

    public void Grow()
    {
        var cell = Instantiate(
            cellPrefab,
            (Vector2)tail,
            Quaternion.identity,
            transform);

        cells.Add(cell);

        UpdateColors();

        void UpdateColors()
        {
            for (int i = 0; i < cells.Count; i++)
                cells[i].sprite.color = rainbow.Get((float)i / (cells.Count - 1));
        }
    }

    public void Stop()
    {
        speed = 0;
    }

    public void ChangeDirection(Vector2Int dir)
    {
        var _dir = Vector2Int.FloorToInt((Vector2)this.dir);
        if (_dir == dir) return;
        if (Vector2.Dot(_dir, dir) == -1) return;

        this.dir = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collisioned.Invoke(other.gameObject.layer);
    }

    public List<Vector2Int> CellPositions()
    {
        return cells.Select(x => x.pos).ToList();
    }
}