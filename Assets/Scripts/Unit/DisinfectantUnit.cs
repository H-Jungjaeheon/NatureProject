using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectantUnit : BasicUnit
{
    [Header("특수능력 확률")]
    [SerializeField] private int StopAttack = 0;
    public override void AttackTime()
    {
        if (Target != null || ECTarget != null)
        {
            StopAttack = Random.Range(0, 101);
            if(Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                if (StopAttack <= 10)
                {
                    Target.GetComponent<BasicEnemy>().StopCount = 1;
                }
                StartCoroutine(Fire(true));
            }
            if(ECTarget != null)
            {
                StartCoroutine(Fire(false));
            }
        }
    }
    IEnumerator Fire(bool isUnit)
    {
        float nowAttackCount = 0;
        while (nowAttackCount < 5)
        {
            if (isUnit)
            {
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            else 
            {
                BattleSceneManager.In.EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            yield return new WaitForSeconds(0.2f);
            nowAttackCount++;
        }
        
    }
}
