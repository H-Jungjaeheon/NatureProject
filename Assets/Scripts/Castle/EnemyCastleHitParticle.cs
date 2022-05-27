using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastleHitParticle : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ParticleDestroy());
    }
    private IEnumerator ParticleDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
