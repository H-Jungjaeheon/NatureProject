using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Trash : MonoBehaviour
{
    [SerializeField]
    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;

            if (hp <= 0)
            {
                DestroyObject();
            }
        }
    }

    [SerializeField]
    int HpRandomRange;

    private void Start()
    {
        HP =  3 + Random.Range(-HpRandomRange, HpRandomRange + 1);
    }

    public void OnHit()
    {
        HP--;
    }

    void DestroyObject()
    {
        // 효과 넣어주면 됨

        Destroy(this.gameObject);
    }
}
