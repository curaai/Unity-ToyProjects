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

    private InputManager input;
    private Vector3 dir;
    public Action<int> collisioned;
    private List<Cell> cells = new();
    private Vector3 tail;

    private void Start()
    {
        input = GetComponent<InputManager>();
        input.onInput += ChangeDirection;
        cells.Add(GetComponentInChildren<Cell>());

        StartCoroutine(_Update());
    }

    private IEnumerator _Update()
    {
        while (speed > 0)
        {
            var posList = cells.Select(c => c.transform.position).ToList();

            transform.position += dir;
            for (int i = 1; i < cells.Count; i++)
                cells[i].transform.position = posList[i - 1];

            tail = posList.Last();

            yield return new WaitForSeconds(1 / speed);
        }
    }

    public void Grow()
    {
        var cell = Instantiate(
            cellPrefab,
            tail,
            Quaternion.identity,
            transform);

        cells.Add(cell);
    }

    public void Stop()
    {
        speed = 0;
    }

    private void ChangeDirection(Vector2Int dir)
    {
        var _dir = Vector2Int.FloorToInt((Vector2)this.dir);
        if (_dir == dir) return;
        if (Vector2.Dot(_dir, dir) == -1) return;

        this.dir = (Vector2)dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collisioned.Invoke(other.gameObject.layer);
    }
}