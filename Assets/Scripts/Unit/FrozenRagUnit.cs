using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenRagUnit : BasicUnit
{
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime; ;
        
        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && ECTarget != null)
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
            AttackCount = 0;
            AttackCoolTimeCount = 0;
            Target = null;
            ECTarget = null;
        }
    }
}
