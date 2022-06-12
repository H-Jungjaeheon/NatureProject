using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnemy : BasicEnemy
{
    [Header("발사체")]
    [SerializeField] private GameObject Bullet;
    [Header("발사체 위치 조절")]
    [SerializeField] private Vector3 BulletSpawnVector;
    protected override void AttackTime()
    {
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && PlayerCastle != null)
        {
            if (Target != null || PlayerCastle != null)
            {
                print("소환");
                Instantiate(Bullet, transform.position + BulletSpawnVector, Bullet.transform.rotation);
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
            Target = null;
            PlayerCastle = null;
        }
    }
}
