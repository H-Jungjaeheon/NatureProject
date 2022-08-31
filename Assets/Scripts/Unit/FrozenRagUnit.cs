using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenRagUnit : BasicUnit
{
    public override void AttackTime()
    {
        if (Target != null || ECTarget != null)
        {
            if(Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                Target.GetComponent<BasicEnemy>().MoveSlowCount = 3;
                Target.GetComponent<BasicEnemy>().AttackSlowCount = 1;
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            if(ECTarget != null)
            {
                BattleSceneManager.In.EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
        }
    }
}
