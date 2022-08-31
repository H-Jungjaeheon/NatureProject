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
        if (Target != null && PlayerCastle != null)
        {
            if (Target != null || PlayerCastle != null)
            {
                print("소환");
                Instantiate(Bullet, transform.position + BulletSpawnVector, Bullet.transform.rotation);
            }
        }
    }
}
