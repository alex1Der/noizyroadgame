using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private int minCarSpeed;
    [SerializeField] private int maxCarSpeed;

    private int carMovementDuration;
    private float startPosition;
    private bool isCoroutineExecuting;

    private void OnEnable()
    {
        startPosition = transform.position.x;
        carMovementDuration = Random.Range(minCarSpeed, maxCarSpeed);
        transform.DOMoveX(startPosition * -1, carMovementDuration, false);

        StartCoroutine(ExecuteAfterTime(carMovementDuration, () => ObjectPoolScript.Despawn(gameObject)));
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
