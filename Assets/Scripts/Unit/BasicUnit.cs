using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    [SerializeField] private float Hp, MaxHp, Damage, Speed, Range;
    [SerializeField] private bool IsKnockBack;
    [SerializeField] private GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        Dead();
    }
    public virtual void Move()
    {
        transform.position = transform.position + new Vector3(Time.deltaTime * Speed,0,0);
    }
    public virtual void Attack()
    {
        Debug.DrawRay(transform.position, new Vector3(Range, 0, 0), Color.red);
        RaycastHit2D RH = Physics2D.Raycast(transform.position, new Vector3(Range, 0, 0));
        if (RH.collider.CompareTag("Enemy"))
        {
             
        }
    }
    public virtual void Dead()
    {

    }
}
