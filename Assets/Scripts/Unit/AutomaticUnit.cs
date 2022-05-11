using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticUnit : BasicUnit
{
    [Header("특수능력 확률")]
    [SerializeField] private int StopAttack = 0;
    protected override void AttackTime()
    {
        if (IsAttackSlow == true)
            AttackCount += Time.deltaTime / 1.5f;
        else
            AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
            StopAttack = Random.Range(0, 101);
            if (StopAttack <= 20)
                Target.GetComponent<BasicEnemy>().StopCount = 3;
            Target.GetComponent<BasicEnemy>().Hp -= Damage;
            Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
    protected override void AttackCoolTime()
    {
        if (IsAttackSlow == true)
            AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else
            AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position - new Vector3(0,1,0), Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 1, 0), Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //기본 애니 실행
                }
            }
        }
        else
            IsAttackReady = false;
    }
}
