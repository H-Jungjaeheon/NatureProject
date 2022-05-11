using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectantUnit : BasicUnit
{
    [Header("특수능력 확률")]
    [SerializeField] private int StopAttack = 0;
    protected override void AttackTime()
    {
        if (IsAttackSlow == true)
            AttackCount += Time.deltaTime / 1.5f;
        else
            AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
            StopAttack = Random.Range(0, 101);
            if (StopAttack <= 10)
                Target.GetComponent<BasicEnemy>().StopCount = 1;
            Target.GetComponent<BasicEnemy>().Hp -= Damage;
            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
}
