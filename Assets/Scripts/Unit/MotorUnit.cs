using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorUnit : BasicUnit
{
    private RaycastHit2D[] Hit;
    public override void AttackCoolTime()
    {
        if (IsAttackSlow == true)
            AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else
            AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        for (int a = 0; a < Hit.Length; a++)
        {
            RaycastHit2D Hits = Hit[a];
            if (Hits.collider.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackAnim();
                    if (IsAttackSlow == true)
                        AttackCount += Time.deltaTime / 1.5f;
                    else
                        AttackCount += Time.deltaTime;
                    if (AttackCount >= MaxAttackCount)
                    {
                        Hits.collider.GetComponent<BasicEnemy>().Hp -= Damage;
                        Hits.collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;  
                        if(a >= Hit.Length - 1)
                        {
                            AttackCount = 0;
                            AttackCoolTimeCount = 0;
                        }
                    }
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //기본 애니 실행
                }
            }
            else
                IsAttackReady = false;
        }
    }
}
