using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("À¯´Ö °ü·Ã º¯¼ö")]
    public float Hp, ReceivDamage;
    [SerializeField] private float MaxHp, Damage, MaxReceivDamage, Speed, Range, KnockBackCount, AttackCount, MaxAttackCount;
    public bool IsKnockBack;
    [SerializeField] private bool IsAttack;
    [SerializeField] private GameObject Target;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        MaxReceivDamage = MaxHp / KnockBackCount;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Move();
    }
    public virtual void FixedUpdate()
    {
        Attack();
        StatManagement();
        KnockBack();
    }
    public virtual void StatManagement()
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;
    }
    public virtual void Attack()
    {
        AttackCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicUnit>().IsKnockBack == false)
            {
                IsAttack = true;
                if (AttackCount >= MaxAttackCount && IsAttack == true)
                {
                    Target.GetComponent<BasicUnit>().Hp -= Damage;
                    Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
                    AttackCount = 0;
                }
            }
        }
        else
            IsAttack = false;
    }
    public virtual void Move()
    {
        if (IsAttack == false)
            transform.position = transform.position - new Vector3(Time.deltaTime * Speed, 0, 0);
    }
    public virtual void Dead()
    {
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            //Á×À½ È¿°ú ¼ÒÈ¯
        }
    }
    public virtual void KnockBack()
    {
        if (ReceivDamage == MaxReceivDamage)
        {
            ReceivDamage = 0;
            Debug.Log("³Ë¹é");
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            Debug.Log("³Ë¹é");
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
    }
    public virtual IEnumerator KnockBacking()
    {
        if(IsKnockBack == true)
        {
            float KnockBackUpSpeed = 200, KnockBackBackSpeed = 180;
            rigid.AddForce(Vector2.right * KnockBackBackSpeed);
            rigid.AddForce(Vector2.up * KnockBackUpSpeed);
            yield return new WaitForSeconds(0.3f);
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2));
            yield return new WaitForSeconds(0.3f);
            rigid.AddForce(Vector2.up * ((KnockBackUpSpeed) * 2f));
            yield return new WaitForSeconds(0.2f);
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2f));
            yield return new WaitForSeconds(0.2f);
            rigid.velocity = Vector3.zero;
        }
        IsKnockBack = false;
        if(Hp <= 0)
            Dead();
        yield return null;
    }
}
