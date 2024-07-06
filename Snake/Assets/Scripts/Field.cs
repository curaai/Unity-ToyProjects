using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Snake snake;
    [SerializeField] private GameObject fruitPrefab;

    private GameObject curFruit;

    private void Start()
    {
        transform.localScale = (Vector2)size;

        snake.collisioned += OnSnakeTrigger;

        GenerateFruit();
    }

    private void GenerateFruit()
    {
        curFruit = Instantiate(fruitPrefab);
        curFruit.transform.position = new Vector2(
            Random.Range(-size.x / 2 + 1, size.x / 2),
            Random.Range(-size.y / 2 + 1, size.y / 2)
        );
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
    }
}