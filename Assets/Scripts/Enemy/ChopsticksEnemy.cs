using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopsticksEnemy : BasicEnemy
{
    protected override void AttackCoolTime()
    {
        if (IsAttackSlow == true) AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        RaycastHit2D Castlehit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("PlayerCastle"));

        PlayerCastle = (Castlehit.collider != null) ? PlayerCastle = Castlehit.collider.gameObject : PlayerCastle = null;
        if (PlayerCastle != null || hit.Length > 0 && hit[0].collider != null)
        {
            IsAttackReady = true;
            if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                IsAttackReady = false;
                StartCoroutine(AttackAnim());
                for (int b = 0; b < hit.Length; b++)
                {
                    Target = (hit[b] && hit[b].collider != null) ? Target = hit[b].collider.gameObject : Target = null;

                    if (Target && Target.GetComponent<BasicUnit>().IsKnockBack == false)
                    {
                        Target.GetComponent<BasicUnit>().Hp -= Damage;
                        Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
                        Target = null;
                    }
                }
                if (PlayerCastle)
                {
                    BGameManager.GetComponent<BattleSceneManager>().PlayerHp -= Damage;
                    //PlayerCastle.GetComponent<EnemyCastle>().IsHit = true;
                }
                AttackCoolTimeCount = 0;
                hit = null;
                PlayerCastle = null;
            }
            else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
            {
                //기본 애니 실행
            }
        }
        else IsAttackReady = false;
    }
}
