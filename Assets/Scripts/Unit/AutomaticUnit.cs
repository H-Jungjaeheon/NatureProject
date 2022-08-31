using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticUnit : BasicUnit
{
    [Header("Ư���ɷ� Ȯ��")]
    [SerializeField] private int StopAttack = 0;
    public override void AttackTime()
    {
        if (Target != null || ECTarget != null)
        {
            StopAttack = Random.Range(0, 101);
            if (Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                if (StopAttack <= 20)
                {
                    Target.GetComponent<BasicEnemy>().StopCount = 3;
                }
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            if (ECTarget != null)
            {
                BattleSceneManager.In.EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            AttackCoolTimeCount = 0;
            Target = null;
            ECTarget = null;
        }
    }
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position - new Vector3(0,1,0), Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 1, 0), Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position - new Vector3(0, 1, 0), Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (hit.collider != null || castlehit.collider != null && castlehit)
        {
            Target = (hit.collider != null) ? Target = hit.collider.gameObject : Target = null;
            ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : ECTarget = null;

            if (Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false || ECTarget)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady)
                {
                    //�⺻ �ִ� ����
                }
            }
            else IsAttackReady = false;
        }
        else IsAttackReady = false;
    }
}
