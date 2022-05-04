using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    [Header("À¯´Ö °ü·Ã º¯¼ö")]
    public float Hp;
    public float ReceivDamage, StopCount, AttackCount, MaxAttackCount, Range, Damage;
    [SerializeField] private float MaxHp, MaxReceivDamage, Speed, KnockBackCount;
    public bool IsKnockBack, IsStop, IsAttack;
    public GameObject Target;// DeadEffect;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        StartCoroutine(FirstSpawnAnim());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(IsKnockBack == false && IsStop == false)
           Move();
        Stops();
    }
    private void FixedUpdate()
    {
        if(IsStop == false && IsKnockBack == false)
           Attack();
        StatManagement();
        KnockBack();
    }
    public virtual void Stops()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else
            IsStop = false;
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
        Dead();
    }
    public virtual IEnumerator FirstSpawnAnim()
    {
        IsKnockBack = true;
        rigid.AddForce(new Vector2(80, 110));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(80, -213));
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
        yield return null;
    }
    public virtual IEnumerator KnockBacking()
    {
        if (IsKnockBack == true)
        {
            float KnockBackUpSpeed = 170, KnockBackBackSpeed = 150;
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
            //Instantiate(DeadEffect, transform.position, DeadEffect.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
