using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay = 2.0f;

    private ObjectPoolManager objectPoolManager = null;

    public void OnEnable()
    {
        StartCoroutine(Recycle());
    }

    private IEnumerator Recycle()
    {
        objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        yield return new WaitForSeconds(delay);
        objectPoolManager.RecycleObject(gameObject);
    }
}
