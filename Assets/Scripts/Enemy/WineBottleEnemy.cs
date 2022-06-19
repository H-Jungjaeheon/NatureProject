using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottleEnemy : BasicEnemy
{
    [Header("ÀÚÆø µô·¹ÀÌ")]
    [SerializeField] private float BoomDelay;
    protected override void KnockBack()
    {
        if (Hp <= 0)
        {
            StartCoroutine(DeadBoom());
        }
        else if (ReceivDamage == MaxReceivDamage)
        {
            ReceivDamage = 0;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            StartCoroutine(KnockBacking());
        }
        else if (IsSuctioning == true && Hp <= 0)
        {
            Dead();
        }
    }
    private IEnumerator DeadBoom()
    {
        IsKnockBack = true;
        yield return new WaitForSeconds(BoomDelay);
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
        Dead();
    }
}
