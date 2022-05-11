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
        IsRush = true;
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        StartCoroutine(FirstSpawnAnim());
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsKnockBack == false && IsStop == false && Hp > 0) Move();
        if (Hp > 0) Stops();
    }
    protected override void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false && IsRush == false && Hp > 0) AttackCoolTime();
        else if (IsStop == false && IsKnockBack == false && IsRush == true && Hp > 0) RushAttackCoolTime();
        StatManagement();
        KnockBack();
        Dead();
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
        rigid.AddForce(new Vector2(80, 110));
        yield return new WaitForSeconds(0.3f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(80, -213));
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        IsKnockBack = false;
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
        for (int a = 0; a < Hit.Length; a++)
        {
            RaycastHit2D Hits = Hit[a];
            if (Hits.collider.GetComponent<BasicEnemy>().IsKnockBack == false && Hp > 0)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackAnim();
                    AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                    if (AttackCount >= MaxAttackCount)
                    {
                        if (Hits.collider.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Hits.collider.GetComponent<BasicEnemy>().Hp -= Damage;
                            Hits.collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                            Hits.collider.GetComponent<BasicEnemy>().IsPush = true;
                            IsRush = false;
                        }
                        if (a >= Hit.Length - 1)
                        {
                            AttackCount = 0;
                            AttackCoolTimeCount = 0;
                        }
                    }
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true && Hp > 0)
                {
                    //기본 애니 실행
                }
            }
            else
                IsAttackReady = false;
        }
    }
    protected override void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false && Hp > 0)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackTime();
                    AttackAnim();
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //기본 애니 실행
                }
            }
        }
        else
            IsAttackReady = false;
    }
    protected override void AttackAnim()
    {
        if (IsAttackAnim == false && Hp > 0)
        {
            IsAttackAnim = true;
            //공격 애니 실행
        }
    }
    protected override void AttackAnimStop() => IsAttackAnim = false; //공격 모션 캔슬 or 끝날 시 실행 함수
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount && Hp > 0)
        {
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                Target.GetComponent<BasicEnemy>().Hp -= Damage;
                Target.GetComponent<BasicEnemy>().ReceivDamage += Damage;
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
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
            if (SuctionHits.collider.GetComponent<BasicEnemy>().IsKnockBack == false && SuctionHits.collider.GetComponent<BasicEnemy>().IsBoss == false)
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
            if (InstantDeath.collider.GetComponent<BasicEnemy>().IsKnockBack == false && InstantDeath.collider.GetComponent<BasicEnemy>().IsBoss == false)
            {
                AttackAnim();
                InstantDeath.collider.gameObject.GetComponent<BasicEnemy>().Hp = 0;
                InstantDeathCount++;
            }
        }
    }
}
