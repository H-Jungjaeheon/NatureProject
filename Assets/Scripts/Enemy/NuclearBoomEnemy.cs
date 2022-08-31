using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBoomEnemy : BasicEnemy
{
    [SerializeField] private bool IsAttackGetReady;
    protected override void AttackCoolTime()
    {
        if (IsAttackSlow == true) AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        RaycastHit2D Castlehit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("PlayerCastle"));
        if (hit.collider != null || Castlehit.collider != null)
        {
            Target = (hit.collider != null) ? hit.collider.gameObject : null;
            PlayerCastle = (Castlehit.collider != null) ? Castlehit.collider.gameObject : null;

            if (Target != null && Target.GetComponent<BasicUnit>().IsKnockBack == false && Target.GetComponent<BasicUnit>().IsSuction == false || PlayerCastle != null)
            {
                IsAttackGetReady = true;
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady || IsAttackGetReady)
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
    protected override void AttackTime()
    {
        if (Target != null && Target.GetComponent<BasicUnit>().IsKnockBack == false)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
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
        }
        if (PlayerCastle != null)
        {
            BGameManager.GetComponent<BattleSceneManager>().PlayerHp -= Damage;
            //PlayerCastle.GetComponent<PlayerCastle>().IsHit = true;
        }
        Hp -= MaxHp;
        Dead();
    }
    protected override void KnockBack()
    {
        if (ReceivDamage == MaxReceivDamage)
        {
            IsAttackGetReady = false;
            ReceivDamage = 0;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            IsAttackGetReady = false;
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            StartCoroutine(KnockBacking());
        }
        else if (IsSuctioning == true && Hp <= 0)
        {
            IsAttackGetReady = false;
            Dead();
        }
    }
}
