using System.Collections;
using UnityEngine;

public class MonoBehaviourExtended : MonoBehaviour
{

    protected private void StopStartCoroutine(Coroutine coroutine, IEnumerator unumerator)
    {
        StopRoutine(coroutine);
        coroutine = StartCoroutine(unumerator);
    }
    protected private void StopRoutine(Coroutine coroutine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }


    protected private void ClearChildren() => ClearChildren(transform);
    protected private void ClearChildren(Transform transformParent)
    {
        while (transformParent.childCount > 0)
            foreach (Transform transformChild in transformParent)
                DestroyImmediate(transformChild.gameObject);
    }

    protected private float GetRandomValue(Vector2 minMax) => Random.Range(minMax.x, minMax.y);
    protected private int GetRandomValue(Vector2Int minMax) => Random.Range(minMax.x, minMax.y);
}
