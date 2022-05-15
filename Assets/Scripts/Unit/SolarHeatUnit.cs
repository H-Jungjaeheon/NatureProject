using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarHeatUnit : BasicUnit
{
    private RaycastHit2D[] Hits;
    private RaycastHit2D Hit;
    [SerializeField] private int MaxLazerHitCount;
    private readonly WaitForSeconds WFS = new WaitForSeconds(1f);
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        Hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        Hits = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (Hit && Hit.collider.GetComponent<BasicEnemy>().IsKnockBack == false)
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
        else IsAttackReady = false;
    }
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {           
           StartCoroutine(Attack());
           AttackCount = 0;
           AttackCoolTimeCount = 0;            
        }
    }
    private IEnumerator Attack()
    {
        AttackAnim();
        for (int LazerHitCount = 0; LazerHitCount <= MaxLazerHitCount; LazerHitCount++)
        {
            for (int a = 0; a < Hits.Length; a++)
            {
                if (Hits[a].collider.GetComponent<BasicEnemy>().IsKnockBack == false)
                {
                    Hits[a].collider.GetComponent<BasicEnemy>().Hp -= Damage;
                    Hits[a].collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                }               
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
