using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorUnit : BasicUnit
{
    private RaycastHit2D[] Hit;
    //[SerializeField] private GameObject[] Targets;
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (castlehit.collider != null) ECTarget = castlehit.collider.gameObject;
        else ECTarget = null;

        //for (int NowHitLength = 0; NowHitLength < Hit.Length; NowHitLength++)
        //{
        //    if (Hit[NowHitLength] && Hit[NowHitLength].collider != null) Targets[NowHitLength] = Hit[NowHitLength].collider.gameObject;
        //    else Targets[NowHitLength] = null;
        //}

        for (int a = 0; a < Hit.Length; a++)
        {
            if (Hit[a].collider.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackAnim();
                    AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                    if (AttackCount >= MaxAttackCount)
                    {
                        if (Hit[a].collider.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Hit[a].collider.GetComponent<BasicEnemy>().Hp -= Damage;
                            Hit[a].collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                        }
                        if (a >= Hit.Length - 1)
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