using System.Collections;
using UnityEngine;

public class YieldCollection : CustomYieldInstruction
{
    int _count;

    public IEnumerator CountCoroutine(IEnumerator coroutine)
    {
        _count++;
        yield return coroutine;
        _count--;
    }

    public override bool keepWaiting => _count != 0;
}
