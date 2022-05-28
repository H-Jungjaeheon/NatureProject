using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public bool IsHit;
    [SerializeField] private int HitCount;
    [SerializeField] private SpriteRenderer SR, CSR;
    [SerializeField] private Sprite[] CastleSprites, TrashBoxCapSprites;
    [SerializeField] private GameObject HitParticle, CastleBodyObj, TrashBoxCap;
    private void Awake()
    {
        SR = CastleBodyObj.GetComponent<SpriteRenderer>();
        CSR = TrashBoxCap.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (IsHit) HitEffect();
        CastleSprite();
    }
    private void CastleSprite()
    {
        if(BattleSceneManager.In.EnemyHp >= (BattleSceneManager.In.MaxEnemyHp / 3) * 2)
        {
            SR.sprite = CastleSprites[0];
            CSR.sprite = TrashBoxCapSprites[0];
        }
        else if(BattleSceneManager.In.EnemyHp >= (BattleSceneManager.In.MaxEnemyHp / 3) * 1.5f)
        {
            SR.sprite = CastleSprites[1];
            CSR.sprite = TrashBoxCapSprites[1];
        }
        else
        {
            SR.sprite = CastleSprites[2];
            CSR.sprite = TrashBoxCapSprites[2];
        }
    }
    private void HitEffect()
    {
        IsHit = false;
        HitCount++;
        StartCoroutine(CastleHitShake());
        if(HitCount >= 5)
        {
            Instantiate(HitParticle, transform.position + new Vector3(Random.Range(-1.53f, 1.48f), Random.Range(0.7f, -1.11f), 0), HitParticle.transform.rotation);
            HitCount = 0;
        }
    }
    private IEnumerator CastleHitShake()
    {
        WaitForSeconds WFS = new WaitForSeconds(0.05f);
        CastleBodyObj.transform.position += new Vector3(0.05f, 0, 0);
        yield return WFS;
        CastleBodyObj.transform.position -= new Vector3(0.05f, 0, 0);
        yield return null;
    }
}
