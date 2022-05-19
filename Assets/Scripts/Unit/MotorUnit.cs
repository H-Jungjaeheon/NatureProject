using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorUnit : BasicUnit
{
    private RaycastHit2D[] Hit;
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (castlehit.collider != null) ECTarget = castlehit.collider.gameObject;
        else ECTarget = null;

        if (ECTarget != null || Hit != null)
        {
            for (int a = 0; a < Hit.Length; a++)
            {
                RaycastHit2D NowHits = Hit[a];
                if (NowHits && NowHits.collider != null)
                {
                    Target = NowHits.collider.gameObject;
                }
                else
                {
                    Target = null;
                }

                if (Hit[a].collider.GetComponent<BasicEnemy>().IsKnockBack == false || ECTarget != null)
                {
                    NowHits = new RaycastHit2D();
                    IsAttackReady = true;
                    if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                    {
                        AttackAnim();
                        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                        if (AttackCount >= MaxAttackCount && Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Target.GetComponent<BasicEnemy>().Hp -= Damage;
                            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                            Target = null;
                            if (a == Hit.Length - 1)
                            {
                                Target = null;
                                //Hit = null;
                            }
                        }
                    }
                    else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                    {
                        //기본 애니 실행
                    }
                }
            }
            if (ECTarget)
            {
                BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
                ECTarget = null;
                AttackCount = 0;
            }
        }
    }
}