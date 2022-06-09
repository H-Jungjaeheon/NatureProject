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
    [SerializeField] protected GameObject Target, PlayerCastle, EnemyCastle, BGameManager;
    [SerializeField] protected float StartY;
    Rigidbody2D rigid;
    public bool IsBoss;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        EnemyCastle = GameObject.Find("EnemyCastle");
        BGameManager = GameObject.Find("BattleSceneManagerObj");
        MaxReceivDamage = MaxHp / KnockBackCount;
        BossKnockBackStart();
        StartCoroutine(FirstSpawnAnim());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(IsKnockBack == false && IsStop == false)
            Move();
        Debuffs();
    }
    protected virtual IEnumerator FirstSpawnAnim()
    {
        IsKnockBack = true;
        rigid.AddForce(new Vector2(-110, 110));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(-110, -213));
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
        StartY = transform.position.y;
        yield return null;
    }
    protected virtual void BossKnockBackStart()
    {
        if(IsBoss == true && BattleSceneManager.In.BossKnockBackEnemy != null)
        {
            print("넉백");
            for (int a = 0; a < BattleSceneManager.In.BossKnockBackEnemy.Length - 1; a++)
            {
                BattleSceneManager.In.BossKnockBackEnemy[a].GetComponent<BasicUnit>().IsBossKnockBack = true;
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false) AttackCoolTime();
        StatManagement();
        KnockBack();
        PositionLimit();
        if(IsPush == true && IsKnockBack == false) StartCoroutine(Pushing());
        if (IsPushing == true && IsKnockBack == false)
        {
            Pushings();
            PushSpeed -= Time.deltaTime;
        }
        
    }
    protected virtual void Pushings() => rigid.AddForce(new Vector2(PushSpeed, 0));
    protected virtual void PositionLimit()
    {
        if(transform.position.x > EnemyCastle.transform.position.x + 1.5f && IsBoss == false)
        {
            transform.position = new Vector3(EnemyCastle.transform.position.x + 1.5f, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > EnemyCastle.transform.position.x - 2.7f && IsBoss)
        {
            transform.position = new Vector3(EnemyCastle.transform.position.x - 2.7f, transform.position.y, transform.position.z);
        }
    }
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
    protected virtual void Debuffs()
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
    protected virtual void StatManagement()
    {
        if (Hp >= MaxHp) Hp = MaxHp;
        if (StopCount <= 0) StopCount = 0;
    }
    protected virtual void AttackCoolTime()
    {
        if(IsAttackSlow == true) AttackCoolTimeCount += Time.deltaTime / 1.5f;
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
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if(AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady)
                {
                    //기본 애니 실행
                }
            }
        }
        else IsAttackReady = false;
    }
    protected virtual void AttackAnim()
    {
        if(IsAttackAnim == false)
        {
            IsAttackAnim = true;
            //공격 애니 실행
        }
    }
    protected virtual void AttackAnimStop() => IsAttackAnim = false; //공격 모션 캔슬 or 끝날 시 실행 함수
    protected virtual void AttackTime()
    {
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && PlayerCastle != null)
        {
            if (Target != null && Target.GetComponent<BasicUnit>().IsKnockBack == false)
            {
                Target.GetComponent<BasicUnit>().Hp -= Damage;
                Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
            }
            if (PlayerCastle != null)
            {
                BGameManager.GetComponent<BattleSceneManager>().PlayerHp -= Damage;
                //PlayerCastle.GetComponent<PlayerCastle>().IsHit = true;
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
    protected virtual void Move()
    {
        //이동 애니 실행
        if (IsAttackReady == false && IsMoveSlow == false) transform.position = transform.position - new Vector3(Time.deltaTime * Speed, 0, 0);
        else if(IsAttackReady == false && IsMoveSlow == true) transform.position = transform.position - new Vector3(Time.deltaTime * 0.1f, 0, 0);
    }
    protected virtual void Dead()
    {
        IsPush = false;
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            //죽음 효과 소환
        }
    }
    protected virtual void KnockBack()
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
    protected virtual IEnumerator KnockBacking()
    {
        IsPush = false;
        AttackCount = 0;
        IsKnockBack = true;
        AttackAnimStop();
        if (IsKnockBack)
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
