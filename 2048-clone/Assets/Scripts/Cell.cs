using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellView view;
    [SerializeField] private Animator anim;

    public Vector2Int cellPos;
    public int value;
    public bool mergeable;

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

    public void Merged()
    {
        anim.SetTrigger("merge");
        Set(cellPos, value * 2);
    }
}