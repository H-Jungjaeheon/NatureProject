using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    #region 유닛 변수
    [Header("유닛 관련 변수")]
    public float Hp;
    [SerializeField] protected float Speed, Range, Damage, MaxHp; //캐릭터 이동속도, 적 인식 사거리, 공격 데미지, 최대체력
    [SerializeField] protected GameObject Target, ECTarget, BGameManager, Castle; //적 타켓, 적 성 타겟

    [Header("유닛이 걸린 상태이상 변수")]
    [SerializeField] protected float StopCount; //상태이상 시간 : 정지
    [SerializeField] protected float AttackSlowCount, MoveSlowCount; //상태이상 시간 : 공격속도 감소, 이동속도 감소
    [SerializeField] protected bool IsStop, IsAttackSlow, IsMoveSlow, IsAttackReady; //정지 상태, 공격속도 감소, 이동속도 감소, 공격 가능 판별

    [Header("넉백 관련 변수")]
    [SerializeField] protected float MaxReceivDamage; //최대 넉백 데미지
    [SerializeField] protected float KnockBackCount; //넉백 횟수
    public float ReceivDamage; //현재 넉백 데미지
    public bool IsKnockBack, IsSuction, IsBossKnockBack, IsStartAnim; //넉백, 빨아들임, 보스 등장 넉백 판별

    [Header("공격 준비 쿨타임")]
    [SerializeField] protected float AttackCoolTimeCount;
    [SerializeField] protected float MaxAttackCoolTimeCount;

    [Header("공격 실행 쿨타임")]
    [SerializeField] protected float AttackCount;
    [SerializeField] protected float MaxAttackCount;

    [Header("그 외")]
    [SerializeField] protected bool IsAttackAnim;
    [SerializeField] protected float StartY;
    [SerializeField] protected int UnitID, LevelPerHp, LevelPerDamage;
    protected Rigidbody2D rigid;
    public Vector3 SpawnVector;
    #endregion
    protected virtual void Start()
    {
        if (GameManager.In.GameUnitData[UnitID - 1].UnitLevel > 1)
        {
            Hp += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerHp);
            MaxHp += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerHp);
            Damage += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerDamage);
        }
        BGameManager = GameObject.Find("BattleSceneManagerObj");
        Castle = GameObject.Find("PlayerCastle");
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        IsStartAnim = true;
        IsKnockBack = true;
        StartCoroutine(FirstSpawnAnim());
    }

    protected virtual void Update()
    {
        if (IsKnockBack == false && IsStop == false)
            Move();
        Stops();
    }
    protected virtual void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false)
            AttackCoolTime();
        StatManagement();
        KnockBack();
        MoveLimit();
        BossKnockBack();
    }
    protected virtual void BossKnockBack()
    {
        if (IsBossKnockBack && IsStartAnim == false)
        {
            print("실행");
            IsBossKnockBack = false;
            IsKnockBack = true;
            StartCoroutine(BossKnockBacking());
        }
    }
    protected virtual void MoveLimit()
    {
        if (transform.position.x < Castle.transform.position.x - 3)
        {
            transform.position = new Vector3(Castle.transform.position.x - 3, transform.position.y, transform.position.z);
        }
    }
    protected virtual void Stops()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else IsStop = false;
    }
    protected virtual void KnockBack()
    {
        if (ReceivDamage == MaxReceivDamage)
        {
            ReceivDamage = 0;
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
        else if (ReceivDamage > MaxReceivDamage)
        {
            ReceivDamage = ReceivDamage % MaxReceivDamage;
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
    }
    protected virtual IEnumerator FirstSpawnAnim()
    {
        rigid.AddForce(new Vector2(110, 110));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(110, -213));
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
        IsStartAnim = false;
        StartY = transform.position.y;
        yield return null;
    }
    protected virtual IEnumerator KnockBacking()
    {
        WaitForSeconds Wait = new WaitForSeconds(0.27f);
        WaitForSeconds Wait2 = new WaitForSeconds(0.17f);
        float KnockBackUpSpeed = 170, KnockBackBackSpeed = 150;

        rigid.AddForce(Vector2.left * KnockBackBackSpeed);
        rigid.AddForce(Vector2.up * KnockBackUpSpeed);
        yield return Wait;
        rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2));
        yield return Wait;
        rigid.AddForce(Vector2.up * ((KnockBackUpSpeed) * 2f));
        yield return Wait2;
        rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2f));
        yield return Wait2;
        rigid.velocity = Vector3.zero;
        IsKnockBack = false;
        transform.position = new Vector3(transform.position.x, StartY, transform.position.z);
        if (Hp <= 0) Dead();
        yield return null;
    }
    protected virtual IEnumerator BossKnockBacking()
    {
        WaitForSeconds Wait = new WaitForSeconds(0.47f);
        WaitForSeconds Wait2 = new WaitForSeconds(0.27f);
        float KnockBackUpSpeed = 230, KnockBackBackSpeed = 170;

        rigid.AddForce(Vector2.left * KnockBackBackSpeed);
        rigid.AddForce(Vector2.up * KnockBackUpSpeed);
        yield return Wait;
        rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2));
        yield return Wait;
        rigid.AddForce(Vector2.up * ((KnockBackUpSpeed) * 2f));
        yield return Wait2;
        rigid.AddForce(Vector2.down * ((KnockBackUpSpeed) * 2f));
        yield return Wait2;
        rigid.velocity = Vector3.zero;
        IsKnockBack = false;
        transform.position = new Vector3(transform.position.x, StartY, transform.position.z);
        if (Hp <= 0) Dead();
        yield return null;
    }
    protected virtual void StatManagement()
    {
        if (Hp >= MaxHp) Hp = MaxHp;
        if (StopCount <= 0) StopCount = 0;
    }

    protected virtual void Move()
    {
        //이동 애니 실행
        if (IsAttackReady == false && IsMoveSlow == false)
            transform.position = transform.position + new Vector3(Time.deltaTime * Speed, 0, 0);
        else if (IsAttackReady == false && IsMoveSlow)
            transform.position = transform.position + new Vector3(Time.deltaTime * 0.1f, 0, 0);
    }
    protected virtual void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;

        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (hit.collider != null || castlehit.collider != null)
        {
            Target = (hit.collider != null) ? Target = hit.collider.gameObject : null;
            ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : null;

            if (Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false || ECTarget)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
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
    protected virtual void AttackAnim()
    {
        if (IsAttackAnim == false)
        {
            IsAttackAnim = true;
            //공격 애니 실행
        }
    }
    protected virtual void AttackAnimStop() => IsAttackAnim = false; //공격 모션 캔슬 or 끝날 시 실행 함수
    protected virtual void AttackTime()
    {
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;

        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && ECTarget != null)
        {
            if (Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            if (ECTarget != null)
            {
                BGameManager.GetComponent<BattleSceneManager>().EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
            Target = null;
            ECTarget = null;
        }
    }
    protected virtual void Dead()
    {
        if (Hp <= 0)
        {
            //Instantiate(DeadEffect, transform.position, DeadEffect.transform.rotation);
            Destroy(this.gameObject);
            //죽음 효과 소환
        }
    }
}