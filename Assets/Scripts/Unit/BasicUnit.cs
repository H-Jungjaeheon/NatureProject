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
        //넉백 구현
        if(ReceivDamage == (MaxHp / KnockBackCount))
        {
            ReceivDamage = 0;
        }
        else if(ReceivDamage > (MaxHp / KnockBackCount))
        {
            ReceivDamage = ReceivDamage - (MaxHp / KnockBackCount);
        }
        //체력이 0 이거나 0이하면, 넉백 실행 후 사망처리
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
        //움직임 멈추고 공격 시작
    }
    public virtual void Dead()
    {
        if(Hp <= 0)
        {
            Destroy(this.gameObject);
            //죽음 효과 소환
        }
    }
}
