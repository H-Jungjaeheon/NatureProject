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
        AttackCount = (IsAttackSlow) ? AttackCount += Time.deltaTime / 1.5f : AttackCount += Time.deltaTime;
        if (AttackCount >= MaxAttackCount && Target != null || AttackCount >= MaxAttackCount && PlayerCastle != null)
        {
            if (Target != null || PlayerCastle != null)
            {
                print("��ȯ");
                Instantiate(Bullet, transform.position + BulletSpawnVector, Bullet.transform.rotation);
            }
            AttackCount = 0;
            AttackCoolTimeCount = 0;
            Target = null;
            PlayerCastle = null;
        }
    }
}
