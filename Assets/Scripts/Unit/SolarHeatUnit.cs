using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarHeatUnit : BasicUnit
{
    RaycastHit2D[] Hits;
    RaycastHit2D Hit;
    [SerializeField] private int MaxLazerHitCount;
    [SerializeField] private bool IsAttacking;
    private readonly WaitForSeconds WFS = new WaitForSeconds(1f);
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        Hits = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : ECTarget = null;

        if (Hit.collider != null && Hit.collider.GetComponent<BasicEnemy>().IsKnockBack == false && Hits.Length > 0 || ECTarget != null)
        {
            IsAttackReady = true;
            if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                AttackTime();
            }
            else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                //기본 애니 실행
            }
        }
        else
        {
            if(IsAttacking == false) IsAttackReady = false;
        }
    }
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
           IsAttacking = true;
           StartCoroutine(Attack());
           AttackCount = 0;
           AttackCoolTimeCount = 0;            
        }
    }
    private IEnumerator Attack()
    {
        AttackAnim();
        for (int LazerHitCount = 0; LazerHitCount < MaxLazerHitCount; LazerHitCount++)
        {
            for (int a = 0; a < Hits.Length; a++)
            {
                Target = Hits[a].collider.gameObject;
                if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
                {
                    Target.GetComponent<BasicEnemy>().Hp -= Damage;
                    Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                }
                Target = null;
                AttackCount = 0;
                AttackCoolTimeCount = 0;
            }
            if (ECTarget != null)
            {
                BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            yield return WFS;
        }
        IsAttacking = false;
        AttackCount = 0;
        AttackCoolTimeCount = 0;
        ECTarget = null;
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
