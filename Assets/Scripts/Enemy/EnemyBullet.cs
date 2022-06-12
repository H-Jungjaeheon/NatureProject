using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private int Damage;
    [SerializeField] private bool IsHit;
    private GameObject BattleGM;

    private void Awake() => BattleGM = GameObject.Find("BattleSceneManagerObj");

    private void FixedUpdate()
    {
        BulletMove();
    }

    private void BulletMove() => transform.position = transform.position - new Vector3(Speed * Time.deltaTime, 0, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit") && IsHit == false && collision.gameObject.GetComponent<BasicUnit>().IsKnockBack == false)
        {
            IsHit = true;
            collision.gameObject.GetComponent<BasicUnit>().Hp -= Damage;
            collision.gameObject.GetComponent<BasicUnit>().ReceivDamage += Damage;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("PlayerCastle") && IsHit == false)
        {
            IsHit = true;
            BattleGM.GetComponent<BattleSceneManager>().PlayerHp -= Damage;
            Destroy(gameObject);
        }
    }
}
