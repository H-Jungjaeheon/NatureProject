using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public bool IsHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHit) HitEffect();
    }
    void HitEffect()
    {
        IsHit = false;
        //파티클 생성
        Debug.Log("성 맞음 이펙트");
    }
}
