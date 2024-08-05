using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1.0f;

    public void SetTimerToDelete()
    {
        StartCoroutine(KillParticle());
    }

    private IEnumerator KillParticle()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
