using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenRagUnit : BasicUnit
{
    public override void AttackTime()
    {
        if (IsAttackSlow == true)
            AttackCount += Time.deltaTime / 1.5f;
        else
            AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
            Target.GetComponent<BasicEnemy>().MoveSlowCount = 3;
            Target.GetComponent<BasicEnemy>().AttackSlowCount = 1;
            Target.GetComponent<BasicEnemy>().Hp -= Damage;
            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
}
