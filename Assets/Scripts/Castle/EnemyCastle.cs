using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public bool IsHit;
    [SerializeField] private int HitCount;
    [SerializeField] private GameObject HitParticle;

    void Update()
    {
        if (IsHit) HitEffect();
    }

    void HitEffect()
    {
        IsHit = false;
        HitCount++;
        if(HitCount >= 10)
        {
            Instantiate(HitParticle, transform.position + new Vector3(Random.Range(-1.53f, 1.48f), Random.Range(0.7f, -1.11f), 0), HitParticle.transform.rotation);
            HitCount = 0;
        }
    }
}
