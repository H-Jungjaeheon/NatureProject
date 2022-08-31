using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnemy : BasicEnemy
{
    [SerializeField] private bool IsRush;
    private RaycastHit2D[] RushHits;
    protected override void Start()
    {
        IsRush = true;
        base.Start();
    }
    protected override void StatManagement()
    {
        Hp = (Hp >= MaxHp) ? Hp = MaxHp : Hp = (Hp + 0);
        StopCount = (StopCount <= 0) ? StopCount = 0 : StopCount = (StopCount + 0);
        Damage = (IsRush) ? Damage = 100 : Damage = 80;
        Speed = (IsRush) ? Speed = 4f : Speed = 2.5f;
    }
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        RushHits = Physics2D.RaycastAll(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        RaycastHit2D Castlehit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("PlayerCastle"));

        if (hit.collider != null || Castlehit.collider != null || RushHits.Length > 0 && RushHits[0].collider != null)
        {
            IsAttackReady = true;
            Target = (hit.collider != null) ? hit.collider.gameObject : null;
            PlayerCastle = (Castlehit.collider != null) ? Castlehit.collider.gameObject : null;

            if (Target != null && Target.GetComponent<BasicUnit>().IsKnockBack == false && Target.GetComponent<BasicUnit>().IsSuction == false || PlayerCastle != null)
            {
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
                {
                    if(IsRush == false)
                    {
                        AttackTime();
                        IsAttackReady = false;
                        StartCoroutine(AttackAnim());
                    }
                    else
                    {
                        for (int b = 0; b < RushHits.Length; b++)
                        { 
                            Target = (RushHits[b] && RushHits[b].collider != null) ? Target = RushHits[b].collider.gameObject : Target = null;

                            if (Target && Target.GetComponent<BasicUnit>().IsKnockBack == false && Target.GetComponent<BasicUnit>().IsPushing == false)
                            {
                                Target.GetComponent<BasicUnit>().Hp -= Damage;
                                Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
                                Target.GetComponent<BasicUnit>().IsPush = true;
                                Target = null;
                            }
                        }
                        if (PlayerCastle)
                        {
                            BGameManager.GetComponent<BattleSceneManager>().PlayerHp -= Damage;
                            //PlayerCastle.GetComponent<PlayerCastle>().IsHit = true;
                        }
                    }
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
