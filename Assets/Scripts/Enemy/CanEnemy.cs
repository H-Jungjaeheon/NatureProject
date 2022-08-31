using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnemy : BasicEnemy
{
    [Header("�߻�ü")]
    [SerializeField] private GameObject Bullet;
    [Header("�߻�ü ��ġ ����")]
    [SerializeField] private Vector3 BulletSpawnVector;
    protected override void AttackTime()
    {
        if (Target != null && PlayerCastle != null)
        {
            if (Target != null || PlayerCastle != null)
            {
                print("��ȯ");
                Instantiate(Bullet, transform.position + BulletSpawnVector, Bullet.transform.rotation);
            }
        }
    }
}
