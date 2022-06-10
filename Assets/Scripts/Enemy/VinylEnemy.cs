using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylEnemy : BasicEnemy
{
    protected override IEnumerator FirstSpawnAnim()
    {
        IsKnockBack = true;
        rigid.AddForce(new Vector2(-110, 110));
        yield return new WaitForSeconds(0.4f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(-110, -213));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
        StartY = transform.position.y;
        yield return null;
    }
    protected override void AttackCoolTime()
    {
        if (IsAttackSlow == true) AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position + new Vector3(0, -1.4f, 0), Vector2.left * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-1.4f,0), Vector2.left, Range, LayerMask.GetMask("Unit"));
        RaycastHit2D Castlehit = Physics2D.Raycast(transform.position + new Vector3(0, -1.4f, 0), Vector2.left, Range, LayerMask.GetMask("PlayerCastle"));
        if (hit.collider != null || Castlehit.collider != null)
        {
            Target = (hit.collider != null) ? hit.collider.gameObject : null;
            PlayerCastle = (Castlehit.collider != null) ? Castlehit.collider.gameObject : null;

            if (Target != null && Target.GetComponent<BasicUnit>().IsKnockBack == false && Target.GetComponent<BasicUnit>().IsSuction == false || PlayerCastle != null)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady)
                {
                    //기본 애니 실행
                }
            }
        }
        else IsAttackReady = false;
    }
}
