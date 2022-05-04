using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    public float Hp, ReceivDamage;
    [SerializeField] private float MaxHp, Damage, MaxReceivDamage, Speed, Range, KnockBackCount, AttackCount, MaxAttackCount;
    public bool IsKnockBack;
    [SerializeField] private bool IsAttack;
    [SerializeField] private GameObject Target, DeadEffect;
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
    private void FixedUpdate()
    {
        Attack();
        StatManagement();
        KnockBack();
    }
    public virtual void KnockBack()
    {
        if (ReceivDamage == MaxReceivDamage)
        {
            ReceivDamage = 0;
            Debug.Log("넉백");
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            Debug.Log("넉백");
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
        //체력이 0 이거나 0이하면, 넉백 실행 후 사망처리
        Dead();
    }
    public virtual void FirstSpawnAnim()
    {

    }
    public virtual IEnumerator KnockBacking()
    {
        if (IsKnockBack == true)
        {
            float KnockBackUpSpeed = 210, KnockBackBackSpeed = 200;
            rigid.AddForce(Vector2.left * KnockBackBackSpeed);
            rigid.AddForce(Vector2.up * KnockBackUpSpeed);
            yield return new WaitForSeconds(0.27f);
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2));
            yield return new WaitForSeconds(0.27f);
            rigid.AddForce(Vector2.up * ((KnockBackUpSpeed) * 2f));
            yield return new WaitForSeconds(0.17f);
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2f));
            yield return new WaitForSeconds(0.17f);
            rigid.velocity = Vector3.zero;
        }
        IsKnockBack = false;
        if (Hp <= 0)
            Dead();
        yield return null;
    }
    public virtual void StatManagement()
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;
    }
    public virtual void Move()
    {
        if(IsAttack == false)
           transform.position = transform.position + new Vector3(Time.deltaTime * Speed,0,0);
    }
    public virtual void Attack()
    {
        AttackCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if(hit)
        {
            Target = hit.collider.gameObject;
            if(Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttack = true;
                if (AttackCount >= MaxAttackCount && IsAttack == true)
                {
                    Target.GetComponent<BasicEnemy>().Hp -= Damage;
                    Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                    AttackCount = 0;
                }
            }
        }
        else       
            IsAttack = false;       
    }
    public virtual void Dead()
    {
        if(Hp <= 0)
        {
            Instantiate(DeadEffect, transform.position, DeadEffect.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
