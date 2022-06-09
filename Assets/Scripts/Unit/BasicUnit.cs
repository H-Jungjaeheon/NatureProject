using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    #region ���� ����
    [Header("���� ���� ����")]
    public float Hp;
    [SerializeField] protected float Speed, Range, Damage, MaxHp; //ĳ���� �̵��ӵ�, �� �ν� ��Ÿ�, ���� ������, �ִ�ü��
    [SerializeField] protected GameObject Target, ECTarget, BGameManager, Castle; //�� Ÿ��, �� �� Ÿ��

    [Header("������ �ɸ� �����̻� ����")]
    [SerializeField] protected float StopCount; //�����̻� �ð� : ����
    [SerializeField] protected float AttackSlowCount, MoveSlowCount; //�����̻� �ð� : ���ݼӵ� ����, �̵��ӵ� ����
    [SerializeField] protected bool IsStop, IsAttackSlow, IsMoveSlow, IsAttackReady; //���� ����, ���ݼӵ� ����, �̵��ӵ� ����, ���� ���� �Ǻ�

    [Header("�˹� ���� ����")]
    [SerializeField] protected float MaxReceivDamage; //�ִ� �˹� ������
    [SerializeField] protected float KnockBackCount; //�˹� Ƚ��
    public float ReceivDamage; //���� �˹� ������
    public bool IsKnockBack, IsSuction, IsBossKnockBack, IsStartAnim; //�˹�, ���Ƶ���, ���� ���� �˹� �Ǻ�

    [Header("���� �غ� ��Ÿ��")]
    [SerializeField] protected float AttackCoolTimeCount;
    [SerializeField] protected float MaxAttackCoolTimeCount;

    [Header("���� ���� ��Ÿ��")]
    [SerializeField] protected float AttackCount;
    [SerializeField] protected float MaxAttackCount;

    [Header("�� ��")]
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
            print("����");
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
        //�̵� �ִ� ����
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
                    //�⺻ �ִ� ����
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
            //���� �ִ� ����
        }
    }
    protected virtual void AttackAnimStop() => IsAttackAnim = false; //���� ��� ĵ�� or ���� �� ���� �Լ�
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
            //���� ȿ�� ��ȯ
        }
    }
}