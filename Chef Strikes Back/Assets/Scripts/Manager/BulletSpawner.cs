using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager = null;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var bullet = _objectPoolManager.GetObjectFromPool("Bullet");
            bullet.SetActive(true);
        }
    }
}