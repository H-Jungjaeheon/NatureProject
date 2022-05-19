using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorUnit : BasicUnit
{
    protected override void AttackCoolTime()
    {
        //AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D[] Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (castlehit.collider != null) ECTarget = castlehit.collider.gameObject;
        else ECTarget = null;

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
                        Debug.Log(Target);
                        if (Hit[b] && Hit[b].collider != null)
                        {
                            Target = Hit[b].collider.gameObject;
                        }
                        else
                        {
                            Target = null;
                        }

                        if (Target  && Target.GetComponent<BasicEnemy>().IsKnockBack == false || ECTarget != null)
                        {
                            Target.GetComponent<BasicEnemy>().Hp -= Damage;
                            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                            Target = null;
                            if (b == Hit.Length - 1)
                            {
                                Debug.Log(Hit.Length);
                                if (ECTarget)
                                {
                                    BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                                    ECTarget.GetComponent<EnemyCastle>().IsHit = true;
                                    ECTarget = null;
                                    Hit = null;
                                }
                                Hit = null;
                            }
                        }
                    }
                    AttackCount = 0;
                }
            }
            else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                //�⺻ �ִ� ����
            }
        }
        else
        {
            IsAttackReady = false;
        }
    }
}