using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Trash : MonoBehaviour
{
    [Header("�������� ���� �Ӽ�")]
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

            if (hp == 0)
            {
                StartCoroutine(FadeOut(this.gameObject.GetComponent<SpriteRenderer>()));
            }
        }
    }

    [SerializeField]
    float FadeSpeed;

    [SerializeField]
    GameObject[] SpawnItemType;

    private void Start()
    {
        HP = 1;
        Debug.Log(SpawnItemType.Length);
    }

    public void OnHit()
    {
        HP--;
    }

    void DestroyObject()
    {
        ItemSpawn();
        Destroy(this.gameObject);
    }

    void ItemSpawn()
    {
        int ItemType = Random.Range(0, SpawnItemType.Length);

        Instantiate(SpawnItemType[ItemType], this.gameObject.transform.position, Quaternion.identity);
    }

    IEnumerator FadeOut(SpriteRenderer Image)
    {
        Color color = Image.color;                            //color �� �ǳ� �̹��� ����

        while (true)
        {
            yield return null;
            color.a -= Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            Image.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            if (Image.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
            {
                DestroyObject();
                break;
            }
        }

        yield return null;                                        //�ڷ�ƾ ����
    }
}
