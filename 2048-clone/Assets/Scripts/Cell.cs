using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellView view;

    public Vector2Int cellPos;
    public int value;

    public void Set(Vector2Int _cellPos, int _value)
    {
        cellPos = _cellPos;
        value = _value;
        view.Set(this);
    }

    public IEnumerator Move(Vector3 dstPos)
    {
        var moveDuration = 0.1f;
        var dt = 0f;
        var pos = transform.position;
        while (dt <= moveDuration)
        {
            dt += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, dstPos, dt / moveDuration);
            yield return null;
        }
        transform.position = dstPos;
    }
}