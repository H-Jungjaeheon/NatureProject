using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VacuumCleanerCarUnit : BasicUnit
{
    [SerializeField] private bool IsRush;
    private RaycastHit2D[] Hit, SuctionHit, InstantDeathHit;
    [SerializeField] private float SuctionCount, MaxSuctionCount, InstantDeathCount, MaxInstantDeathCount;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GameManager.In.GameUnitData[UnitID - 1].UnitLevel > 1)
        {
            Hp += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerHp);
            MaxHp += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerHp);
            Damage += (GameManager.In.GameUnitData[UnitID - 1].UnitLevel * LevelPerDamage);
        }
        IsRush = true;
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        StartCoroutine(FirstSpawnAnim());
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsKnockBack == false && IsStop == false && Hp > 0 && IsCling == false) Move();
        if (Hp > 0) Stops();
    }
    protected override void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false && IsRush == false && IsCling == false) AttackCoolTime();
        else if (IsStop == false && IsKnockBack == false && IsRush == true && IsCling == false) RushAttackCoolTime();
        StatManagement();
        KnockBack();
        Dead();
        Clings();
        BossKnockBack();
    }
    protected override void Stops()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else
            IsStop = false;
    }
    protected override void KnockBack()
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
            IsKnockBack = true;
            StartCoroutine(KnockBacking());
        }
    }
    protected override IEnumerator FirstSpawnAnim()
    {
        transform.DOScale(1.1f, 0.8f).SetEase(Ease.OutSine);
        IsKnockBack = true;
        rigid.AddForce(new Vector2(110, 110));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(110, -213));
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
        StartY = transform.position.y;
        yield return null;
    }
    protected override IEnumerator KnockBacking()
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
        transform.position = new Vector3(transform.position.x, StartY, transform.position.z);
        if (Hp <= 0)
            IsSuction = true;
        yield return null;
    }
    protected override void StatManagement()
    {
        Damage = (IsRush == true) ? Damage = 100 : Damage = 45;
        Speed = (IsRush == true) ? Speed = 2.5f : Speed = 0.4f;
        Hp = (Hp >= MaxHp) ? Hp = MaxHp : Hp = (Hp + 0);
        StopCount = (StopCount <= 0) ? StopCount = 0 : StopCount = (StopCount + 0);
    }

    protected override void Move()
    {
        //이동 애니 실행
        if (IsAttackReady == false && IsMoveSlow == false)
            transform.position = transform.position + new Vector3(Time.deltaTime * Speed, 0, 0);
        else if (IsAttackReady == false && IsMoveSlow == true)
            transform.position = transform.position + new Vector3(Time.deltaTime * 0.1f, 0, 0);
    }
    private void RushAttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Hit = Physics2D.RaycastAll(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : ECTarget = null;

        if (ECTarget != null || Hit.Length > 0 && Hit[0].collider != null)
        {
            IsAttackReady = true;
            if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
            {
                AttackAnim();
                for (int b = 0; b < Hit.Length; b++)
                {
                    Target = (Hit[b] && Hit[b].collider != null) ? Target = Hit[b].collider.gameObject : Target = null;

                    if (Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false && Target.GetComponent<BasicEnemy>().IsPushing == false)
                    {
                        Target.GetComponent<BasicEnemy>().Hp -= Damage;
                        Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                        Target.GetComponent<BasicEnemy>().IsPush = true;
                        Target = null;
                    }
                }
                if (ECTarget)
                {
                    BattleSceneManager.In.EnemyHp -= Damage;
                    ECTarget.GetComponent<EnemyCastle>().IsHit = true;
                }
                Hit = null;
                ECTarget = null;
                IsRush = false;
            }
            else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady)
            {
                //기본 애니 실행
            }
        }
        else
        {
            IsAttackReady = false;
        }
    }
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        RaycastHit2D castlehit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("EnemyCastle"));

        if (hit.collider != null || castlehit.collider != null)
        {
            Target = (hit.collider != null) ? Target = hit.collider.gameObject : Target = null;
            ECTarget = (castlehit.collider != null) ? ECTarget = castlehit.collider.gameObject : ECTarget = null;

            if (Target && Target.GetComponent<BasicEnemy>().IsKnockBack == false && Target.GetComponent<BasicEnemy>().IsPushing == false || ECTarget)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady)
                {
                    AttackTime();
                    StartCoroutine(AttackAnim());
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady)
                {
                    //기본 애니 실행
                }
            }
            else IsAttackReady = false;
        }
        else IsAttackReady = false;
    }
    protected override IEnumerator AttackAnim()
    {
        if (IsAttackAnim == false) yield break;
        IsAttackAnim = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("isAttacking", false);
    }

    public override void AttackTime()
    {
        if (Target != null || ECTarget != null)
        {
            if (Target != null && Target.GetComponent<BasicEnemy>().IsKnockBack == false && Target.GetComponent<BasicEnemy>().IsPushing == false)
            {
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            if (ECTarget != null)
            {
                BattleSceneManager.In.EnemyHp -= Damage;
                ECTarget.GetComponent<EnemyCastle>().IsHit = true;
            }
            AttackCoolTimeCount = 0;
            Target = null;
            ECTarget = null;
        }
    }
    protected override void Dead()
    {
        if (IsSuction == true)
        {
            if (SuctionCount >= MaxSuctionCount || InstantDeathCount >= MaxInstantDeathCount) Destroy(this.gameObject);
            SuctionCount += Time.deltaTime;
            Suction();
            InstantDeath();
            //죽음 효과 소환
        }
    }
    private void Suction()
    {
        SuctionHit = Physics2D.RaycastAll(transform.position, Vector2.right, Range * 2, LayerMask.GetMask("Enemy"));
        for (int a = 0; a < SuctionHit.Length; a++)
        {
            RaycastHit2D SuctionHits = SuctionHit[a];
            if (SuctionHits.collider.GetComponent<BasicEnemy>().IsKnockBack == false && Target.GetComponent<BasicEnemy>().IsPushing == false && SuctionHits.collider.GetComponent<BasicEnemy>().IsBoss == false)
            {
                AttackAnim();
                SuctionHits.collider.GetComponent<BasicEnemy>().IsSuctioning = true;
                SuctionHits.collider.gameObject.transform.position = Vector3.MoveTowards(SuctionHits.collider.gameObject.transform.position,
                new Vector3(transform.position.x, SuctionHits.collider.gameObject.transform.position.y, SuctionHits.collider.gameObject.transform.position.z), Time.deltaTime * 2);
            }
        }
    }
    private void InstantDeath()
    {
        InstantDeathHit = Physics2D.RaycastAll(transform.position, Vector2.right, Range / 2, LayerMask.GetMask("Enemy"));
        for (int a = 0; a < InstantDeathHit.Length; a++)
        {
            RaycastHit2D InstantDeath = InstantDeathHit[a];
            if (InstantDeath.collider.GetComponent<BasicEnemy>().IsKnockBack == false && Target.GetComponent<BasicEnemy>().IsPushing == false && InstantDeath.collider.GetComponent<BasicEnemy>().IsBoss == false)
            {
                AttackAnim();
                InstantDeath.collider.gameObject.GetComponent<BasicEnemy>().Hp = 0;
                InstantDeathCount++;
            }
        }
    }
}
