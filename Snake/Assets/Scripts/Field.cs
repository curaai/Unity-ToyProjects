using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Field : MonoBehaviour
{
    [SerializeField] public Vector2Int size;
    [SerializeField] private Snake snake;
    [SerializeField] private GameObject fruitPrefab;

    public GameObject curFruit;
    private HashSet<Vector2Int> allPosSet = new();
    public Action gameovered;

    private void Start()
    {
        transform.localScale = (Vector2)size;
        for (int i = -size.x / 2 + 1; i < size.x / 2; i++)
            for (int j = -size.y / 2 + 1; j < size.y / 2; j++)
                allPosSet.Add(new Vector2Int(i, j));

        snake.collisioned += OnSnakeTrigger;

        GenerateFruit();
    }

    private void GenerateFruit()
    {
        var available = allPosSet.Except(snake.CellPositions().ToHashSet()).ToList();
        var pos = (Vector2)available[UnityEngine.Random.Range(0, available.Count)];
        curFruit = Instantiate(fruitPrefab, (Vector3)pos, Quaternion.identity);
    }

    private void OnSnakeTrigger(int layer)
    {
        if (layer == LayerMask.NameToLayer("Wall") ||
            layer == LayerMask.NameToLayer("Cell"))
            GameOver();
        else if (layer == LayerMask.NameToLayer("Fruit"))
        {
            Destroy(curFruit.gameObject);
            snake.Grow();
            GenerateFruit();
        }
    }

    private void GameOver()
    {
        snake.Stop();
        gameovered.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}