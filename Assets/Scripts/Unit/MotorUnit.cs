using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorUnit : BasicUnit
{
    protected override void AttackCoolTime()
    {
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D[] Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : ECTarget = null;

        if (ECTarget != null || Hit.Length > 0 && Hit[0].collider != null)
        {
            IsAttackReady = true;
            if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                AttackAnim();
                AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                if (AttackCount >= MaxAttackCount)
                {
                    for (int b = 0; b < Hit.Length; b++)
                    {
                        Target = (Hit[b] && Hit[b].collider != null) ? Target = Hit[b].collider.gameObject : Target = null;

                        if (Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Target.GetComponent<BasicEnemy>().Hp -= Damage;
                            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                            Target = null;
                        }
                    }
                    if (ECTarget)
                    {
                        BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                        ECTarget.GetComponent<EnemyCastle>().IsHit = true;
                    }
                    AttackCount = 0;
                    Hit = null;
                    ECTarget = null;
                }
            }
            else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                //기본 애니 실행
            }
        }
        else
        {
            IsAttackReady = false;
        }
    }
}