using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityAction<Vector2Int> onInput;
    public UnityAction onUndo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            onInput.Invoke(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.S))
            onInput.Invoke(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.D))
            onInput.Invoke(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.W))
            onInput.Invoke(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.R))
            onUndo.Invoke();
    }
}