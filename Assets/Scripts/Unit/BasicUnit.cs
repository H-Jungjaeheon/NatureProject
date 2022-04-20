using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    [SerializeField] private float Hp, MaxHp, Damage, ReceivDamage, Speed, Range, KnockBackCount;
    [SerializeField] private bool IsKnockBack;
    [SerializeField] private GameObject Target;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
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
        //�˹� ����
        if(ReceivDamage == (MaxHp / KnockBackCount))
        {
            ReceivDamage = 0;
        }
        else if(ReceivDamage > (MaxHp / KnockBackCount))
        {
            ReceivDamage = ReceivDamage - (MaxHp / KnockBackCount);
        }
        //ü���� 0 �̰ų� 0���ϸ�, �˹� ���� �� ���ó��
        Dead();
    }
    public virtual void StatManagement()
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;
    }
    public virtual void Move()
    {
        transform.position = transform.position + new Vector3(Time.deltaTime * Speed,0,0);
    }
    public virtual void Attack()
    {
        Debug.DrawRay(transform.position, new Vector3(Range, 0, 0), Color.red);
        RaycastHit2D RH = Physics2D.Raycast(transform.position, new Vector3(Range, 0, 0));
        //if (RH.collider.CompareTag("Enemy") && RH.collider.gameObject != false)
        //{
        //    Target = RH.collider.gameObject;
        //}
        //������ ���߰� ���� ����
    }
    public virtual void Dead()
    {
        if(Hp <= 0)
        {
            Destroy(this.gameObject);
            //���� ȿ�� ��ȯ
        }
    }
}
