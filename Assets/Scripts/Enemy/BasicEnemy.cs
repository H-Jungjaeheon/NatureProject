using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("���� ���� ����")]
    public float Hp;
    public float ReceivDamage, StopCount, AttackSlowCount, MoveSlowCount;
    [SerializeField] private float MaxHp, Damage, MaxReceivDamage, Speed, Range, KnockBackCount;

    [Header("���� �غ� ��Ÿ��")]
    [SerializeField] private float AttackCoolTimeCount;
    [SerializeField] private float MaxAttackCoolTimeCount;

    [Header("���� ���� ��Ÿ��")]
    [SerializeField] private float AttackCount;
    [SerializeField] private float MaxAttackCount;

    public bool IsKnockBack, IsStop, IsAttackSlow, IsMoveSlow;
    [SerializeField] private bool IsAttackReady, IsAttackAnim;
    [SerializeField] private GameObject Target;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(IsKnockBack == false && IsStop == false)
            Move();
        Debuffs();
    }
    public virtual void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false)
            AttackCoolTime();
        StatManagement();
        KnockBack();
    }
    public virtual void Debuffs()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else
            IsStop = false;

        if (MoveSlowCount > 0)
        {
            MoveSlowCount -= Time.deltaTime;
            IsMoveSlow = true;
        }
        else
            IsMoveSlow = false;

        if (AttackSlowCount > 0)
        {
            AttackSlowCount -= Time.deltaTime;
            IsAttackSlow = true;
        }
        else
            IsAttackSlow = false;
    }
    public virtual void StatManagement()
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;
        if (StopCount <= 0)
            StopCount = 0;
    }
    public virtual void AttackCoolTime()
    {
        if(IsAttackSlow == true)
            AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else
            AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicUnit>().IsKnockBack == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if(AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //�⺻ �ִ� ����
                }
            }
        }
        else
            IsAttackReady = false;
    }
    public virtual void AttackAnim()
    {
        if(IsAttackAnim == false)
        {
            IsAttackAnim = true;
            //���� �ִ� ����
        }
    }
    public virtual void AttackAnimStop() => IsAttackAnim = false; //���� ��� ĵ�� or ���� �� ���� �Լ�
    public virtual void AttackTime()
    {
        if (IsAttackSlow == true)
            AttackCount += Time.deltaTime / 1.5f;
        else
            AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount)
        {
            Target.GetComponent<BasicUnit>().Hp -= Damage;
            Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
    public virtual void Move()
    {
        //�̵� �ִ� ����
        if (IsAttackReady == false && IsMoveSlow == false)
            transform.position = transform.position - new Vector3(Time.deltaTime * Speed, 0, 0);
        else if(IsAttackReady == false && IsMoveSlow == true)
            transform.position = transform.position - new Vector3(Time.deltaTime * 0.1f, 0, 0);
    }
    public virtual void Dead()
    {
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            //���� ȿ�� ��ȯ
        }
    }
    public virtual void KnockBack()
    {
        if (ReceivDamage == MaxReceivDamage)
        {
            ReceivDamage = 0;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            StartCoroutine(KnockBacking());
        }
    }
    public virtual IEnumerator KnockBacking()
    {
        AttackCount = 0;
        IsKnockBack = true;
        AttackAnimStop();
        if (IsKnockBack == true)
        {
            float KnockBackUpSpeed = 170, KnockBackBackSpeed = 150;
            rigid.AddForce(Vector2.right * KnockBackBackSpeed);
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
        if(Hp <= 0)
            Dead();
        yield return null;
    }
}
