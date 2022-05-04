using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectantUnit : BasicUnit
{
    [SerializeField] private int StopAttack = 0;
    public override void Attack()
    {
        AttackCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttack = true;
                if (AttackCount >= MaxAttackCount && IsAttack == true)
                {
                    StopAttack = Random.Range(0, 101);
                    if(StopAttack <= 100)
                    {
                        Target.GetComponent<BasicEnemy>().IsStop = true;
                        Target.GetComponent<BasicEnemy>().StopCount = 1;
                    }
                    Target.GetComponent<BasicEnemy>().Hp -= Damage;
                    Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                    AttackCount = 0;
                }
            }
        }
        else
            IsAttack = false;
    }
}
