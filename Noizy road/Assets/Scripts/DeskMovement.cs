using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DeskMovement : MonoBehaviour
{
    [SerializeField] private float deskMovementDuration;
    private float startPosition;
    private bool isCoroutineExecuting;

    private void OnEnable()
    {
        startPosition = transform.position.x;
        transform.DOMoveX(startPosition * -1, deskMovementDuration, false);

        StartCoroutine(ExecuteAfterTime(deskMovementDuration, () => ObjectPoolScript.Despawn(gameObject)));
    }

    IEnumerator ExecuteAfterTime(float time, System.Action task)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        task();
        isCoroutineExecuting = false;
    }
}
