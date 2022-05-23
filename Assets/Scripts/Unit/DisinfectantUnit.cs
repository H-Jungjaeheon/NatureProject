using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectantUnit : BasicUnit
{
    [Header("특수능력 확률")]
    [SerializeField] private int StopAttack = 0;
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;

        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && ECTarget != null)
        {
            StopAttack = Random.Range(0, 101);
            if(Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                if (StopAttack <= 10)
                {
                    Target.GetComponent<BasicEnemy>().StopCount = 1;
                }

                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            if(ECTarget != null)
            {
                BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
            Target = null;
            ECTarget = null;
        }
    }
}
