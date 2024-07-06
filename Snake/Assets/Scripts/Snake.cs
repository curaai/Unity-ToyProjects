using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float speed;

    private InputManager input;
    private Vector3 dir;
    public Action<int> collisioned;

    private void Start()
    {
        input = GetComponent<InputManager>();
        input.onInput += ChangeDirection;

        StartCoroutine(_Update());
    }

    private IEnumerator _Update()
    {
        while (speed > 0)
        {
            transform.position += dir;
            yield return new WaitForSeconds(1 / speed);
        }
    }

    public void Stop()
    {
        speed = 0;
    }

    private void ChangeDirection(Vector2Int dir)
    {
        var _dir = Vector2Int.FloorToInt((Vector2)this.dir);
        if (_dir == dir) return;

        this.dir = (Vector2)dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collisioned.Invoke(other.gameObject.layer);
    }
}