using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CleaningCarUnit : MonoBehaviour
{
    #region 유닛 변수
    [Header("유닛 관련 변수")]
    public float Hp;
    public float Range, Damage;
    [SerializeField] private float MaxHp, Speed;
    public GameObject Target;// DeadEffect;
    private RaycastHit2D[] Hit;

    [Header("유닛이 걸린 상태이상 변수")]
    public float StopCount;
    public float AttackSlowCount, MoveSlowCount;
    public bool IsStop, IsAttackSlow, IsMoveSlow, IsAttackReady;
    [SerializeField] private bool IsRush;

    [Header("넉백 관련 변수")]
    [SerializeField] private float MaxReceivDamage;
    [SerializeField] private float KnockBackCount;
    public float ReceivDamage;
    public bool IsKnockBack;

    [Header("공격 준비 쿨타임")]
    public float AttackCoolTimeCount;
    public float MaxAttackCoolTimeCount;

    [Header("공격 실행 쿨타임")]
    public float AttackCount;
    public float MaxAttackCount;

    [Header("그 외")]
    [SerializeField] private bool IsAttackAnim;
    Rigidbody2D rigid;
    #endregion
    // Start is called before the first frame update
    private void Start()
    {
        IsRush = true;
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        MaxReceivDamage = MaxHp / KnockBackCount;
        StartCoroutine(FirstSpawnAnim());
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsKnockBack == false && IsStop == false) Move();
        Stops();
    }
    private void FixedUpdate()
    {
        if (IsStop == false && IsKnockBack == false && IsRush == false) AttackCoolTime();
        else if (IsStop == false && IsKnockBack == false && IsRush == true) RushAttackCoolTime();
        StatManagement();
        KnockBack();
    }
    private void Stops()
    {
        if (StopCount > 0)
        {
            StopCount -= Time.deltaTime;
            IsStop = true;
        }
        else
            IsStop = false;
    }
    private void KnockBack()
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
    }
    private IEnumerator FirstSpawnAnim()
    {
        transform.DOScale(1, 1f).SetEase(Ease.OutSine);
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
    private IEnumerator KnockBacking()
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
    private void StatManagement()
    {
        Damage = (IsRush == true) ? Damage = 100 : Damage = 45;
        Speed = (IsRush == true) ? Speed = 1.7f : Speed = 0.4f;
        Hp = (Hp >= MaxHp) ? Hp = MaxHp : Hp = (Hp + 0); 
        StopCount = (StopCount <= 0) ? StopCount = 0 : StopCount = (StopCount + 0);
    }

    private void Move()
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
            if (Hits.collider.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                IsAttackReady = true;
                if (AttackCoolTimeCount >= MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    AttackAnim();
                    AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
                    if (AttackCount >= MaxAttackCount)
                    {
                        if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
                        {
                            Hits.collider.GetComponent<BasicEnemy>().Hp -= Damage;
                            Hits.collider.GetComponent<BasicEnemy>().ReceivDamage += Damage;
                            //Target.GetComponent<BasicUnit>().; 밀치기 실행
                        }
                        if (a >= Hit.Length - 1)
                        {
                            AttackCount = 0;
                            AttackCoolTimeCount = 0;
                        }
                    }
                }
                else if (AttackCoolTimeCount < MaxAttackCoolTimeCount && IsAttackReady == true)
                {
                    //기본 애니 실행
                }
            }
            else
                IsAttackReady = false;
        }
    }
    private void AttackCoolTime()
    {
        AttackCoolTimeCount = (IsAttackSlow == true) ? AttackCoolTimeCount += Time.deltaTime / 1.5f : AttackCoolTimeCount += Time.deltaTime;
        Debug.DrawRay(transform.position, Vector2.right * Range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Range, LayerMask.GetMask("Enemy"));
        if (hit)
        {
            Target = hit.collider.gameObject;
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
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
    private void AttackAnim()
    {
        if (IsAttackAnim == false)
        {
            IsAttackAnim = true;
            //공격 애니 실행
        }
    }
    private void AttackAnimStop() => IsAttackAnim = false; //공격 모션 캔슬 or 끝날 시 실행 함수
    private void AttackTime()
    {
        AttackCount = (IsAttackSlow == true) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;       
        if(AttackCount >= MaxAttackCount)
        {
            if (Target.GetComponent<BasicEnemy>().IsKnockBack == false)
            {
                Target.GetComponent<BasicUnit>().Hp -= Damage;
                Target.GetComponent<BasicUnit>().ReceivDamage += Damage;
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
        }
    }
    private void Dead()
    {
        if (Hp <= 0)
        {
            //Instantiate(DeadEffect, transform.position, DeadEffect.transform.rotation);
            Destroy(this.gameObject);
            //죽음 효과 소환
        }
    }
}