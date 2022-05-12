using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarHeatUnit : BasicUnit
{
    private RaycastHit2D[] Hit;
    [SerializeField] private int MaxLazerHitCount;
    private readonly WaitForSeconds WFS = new WaitForSeconds(1f);
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (Hit[0]) IsAttackReady = true;
        else IsAttackReady = false;
        if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
        {
            AttackTime();
        }
        else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
        {
            //기본 애니 실행
        }
    }
    protected override void AttackTime()
    {
        if (IsAttackSlow == true)
            AttackCount += Time.deltaTime / 1.5f;
        else
            AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                StartCoroutine(Attack());
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
    private IEnumerator Attack()
    {
        for(int LazerHitCount = 0; LazerHitCount <= MaxLazerHitCount; LazerHitCount++)
        {
            for (int a = 0; a < Hit.Length; a++)
            {
                RaycastHit2D Hits = Hit[a];
                if (Hits.collider.GetComponent<BasicEnemy>().IsKnockBack == false)
                {
                    AttackAnim();
                    AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                    if (AttackCount >= MaxAttackCount)
                    {
                        if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Hits.collider.GetComponent<BasicEnemy>().Hp -= Damage;
                            Hits.collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                        }                     
                    }
                }
                else
                    IsAttackReady = false;
                AttackCount = 0;
                AttackCoolTimeCount = 0;
            }
            yield return WFS;
        }
        yield return null;
    }
    protected override IEnumerator KnockBacking()
    {
        AttackCount = 0;
        AttackCoolTimeCount = 0;
        StopCoroutine(Attack());
        return base.KnockBacking();
    }
}
