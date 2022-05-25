using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("유닛 관련 변수")]
    public float Hp;
    public float ReceivDamage, StopCount, AttackSlowCount, MoveSlowCount;
    [SerializeField] protected float MaxHp, Damage, MaxReceivDamage, Speed, Range, KnockBackCount, PushSpeed;

    [Header("공격 준비 쿨타임")]
    [SerializeField] protected float AttackCoolTimeCount;
    [SerializeField] protected float MaxAttackCoolTimeCount;

    [Header("공격 실행 쿨타임")]
    [SerializeField] protected float AttackCount;
    [SerializeField] protected float MaxAttackCount;

    [Header("상태 이상 관련 변수")]
    public bool IsKnockBack;
    public bool IsStop, IsAttackSlow, IsMoveSlow, IsPush, IsPushing, IsSuctioning;
    [SerializeField] protected bool IsAttackReady, IsAttackAnim;
    [SerializeField] protected GameObject Target;
    [SerializeField] protected float StartY;
    Rigidbody2D rigid;
    public bool IsBoss;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        StartY = transform.position.y;
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
        if (IsStop == false && IsKnockBack == false) AttackCoolTime();
        StatManagement();
        KnockBack();
        if(IsPush == true) StartCoroutine(Pushing());
        if (IsPushing == true)
        {
            Pushings();
            PushSpeed -= Time.deltaTime;
        }
    }
    protected virtual void Pushings() => rigid.AddForce(new Vector2(PushSpeed, 0));
    protected virtual IEnumerator Pushing()
    {
        IsPush = false;
        IsPushing = true;
        PushSpeed = 25;
        yield return new WaitForSeconds(0.4f);
        rigid.velocity = Vector2.zero;
        IsPushing = false;
        yield return null;
    }
    public virtual void Debuffs()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else IsStop = false;

        if (MoveSlowCount > 0)
        {
            MoveSlowCount -= Time.deltaTime;
            IsMoveSlow = true;
        }
        else IsMoveSlow = false;

        if (AttackSlowCount > 0)
        {
            AttackSlowCount -= Time.deltaTime;
            IsAttackSlow = true;
        }
        else IsAttackSlow = false;
    }
    public virtual void StatManagement()
    {
        if (Hp >= MaxHp) Hp = MaxHp;
        if (StopCount <= 0) StopCount = 0;
    }
    public virtual void AttackCoolTime()
    {
        if(IsAttackSlow == true) AttackCoolTimeCount += Time.deltaTime / 1.5f;
        else AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.left * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Range, LayerMask.GetMask("Unit"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicUnit>().IsKnockBack == false && Target.GetComponent<BasicUnit>().IsSuction == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if(AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //기본 애니 실행
                }
            }
        }
        else IsAttackReady = false;
    }
    public virtual void AttackAnim()
    {
        if(IsAttackAnim == false)
        {
            IsAttackAnim = true;
            //공격 애니 실행
        }
    }
    public virtual void AttackAnimStop() => IsAttackAnim = false; //공격 모션 캔슬 or 끝날 시 실행 함수
    public virtual void AttackTime()
    {
        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
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
        //이동 애니 실행
        if (IsAttackReady == false && IsMoveSlow == false) transform.position = transform.position - new Vector3(Time.deltaTime * Speed, 0, 0);
        else if(IsAttackReady == false && IsMoveSlow == true) transform.position = transform.position - new Vector3(Time.deltaTime * 0.1f, 0, 0);
    }
    public virtual void Dead()
    {
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            //죽음 효과 소환
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
        else if(IsSuctioning == true && Hp == 0)
        {
            Dead();
        }
    }
    public virtual IEnumerator KnockBacking()
    {
        AttackCount = 0;
        IsKnockBack = true;
        AttackAnimStop();
        if (IsKnockBack == true)
        {
            WaitForSeconds Wait = new WaitForSeconds(0.27f);
            WaitForSeconds Wait1 = new WaitForSeconds(0.17f);

            float KnockBackUpSpeed = 170, KnockBackBackSpeed = 150;
            rigid.AddForce(Vector2.right * KnockBackBackSpeed);
            rigid.AddForce(Vector2.up * KnockBackUpSpeed);
            yield return Wait;
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2));
            yield return Wait;
            rigid.AddForce(Vector2.up * ((KnockBackUpSpeed) * 2f));
            yield return Wait1;
            rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2f));
            yield return Wait1;
            rigid.velocity = Vector3.zero;
        }
        transform.position = new Vector3(transform.position.x, StartY, transform.position.z);
        IsKnockBack = false;
        if(Hp <= 0)
            Dead();
        yield return null;
    }
}
