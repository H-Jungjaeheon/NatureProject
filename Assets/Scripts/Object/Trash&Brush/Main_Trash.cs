using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Trash : MonoBehaviour
{
    [Header("쓰레기통 개인 속성")]
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
        Color color = Image.color;                            //color 에 판넬 이미지 참조

        while (true)
        {
            yield return null;
            color.a -= Time.deltaTime * FadeSpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            Image.color = color;                                //판넬 이미지 컬러에 바뀐 알파값 참조

            if (Image.color.a <= 0)                        //만약 판넬 이미지 알파 값이 0보다 작으면
            {
                DestroyObject();
                break;
            }
        }

        yield return null;                                        //코루틴 종료
    }
}
