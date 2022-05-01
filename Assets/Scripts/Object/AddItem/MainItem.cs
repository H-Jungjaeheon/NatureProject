using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MainItem : MonoBehaviour
{
    [Header("메인 아이템 상속 변수")]
    [SerializeField]
    protected int AddCount;
    [SerializeField]
    protected Text AddCountText;
    [SerializeField]
    protected Text PlusText;
    [SerializeField]
    protected float DestroySpeed;
    [SerializeField]
    protected float MoveSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        BasicSetting();

        StartCoroutine(FadeOut(this.gameObject.GetComponent<SpriteRenderer>(), PlusText, AddCountText));
    }

    protected virtual void BasicSetting()
    {
        AddCountText.text = $"X{AddCount}";
    }

    protected IEnumerator FadeOut(SpriteRenderer Image,  Text Txt_Plus, Text Txt_Count)
    {
        Color Imgcolor = Image.color;                            //color 에 판넬 이미지 참조
        Color PlusTxtcolor = Txt_Plus.color;                     //color 에 판넬 이미지 참조
        Color CountTxtcolor = Txt_Count.color;                   //color 에 판넬 이미지 참조

        while (true)
        {
            #region 알파값 조절
            yield return null;

            Imgcolor.a -= Time.deltaTime * DestroySpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            Image.color = Imgcolor;                                //판넬 이미지 컬러에 바뀐 알파값 참조

            PlusTxtcolor.a -= Time.deltaTime * DestroySpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            Txt_Plus.color = PlusTxtcolor;

            CountTxtcolor.a -= Time.deltaTime * DestroySpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            Txt_Count.color = CountTxtcolor;

            if (Image.color.a <= 0)                        //만약 판넬 이미지 알파 값이 0보다 작으면
            {
                DestroyObject();
                break;
            }
            #endregion

            #region 오브젝트 이동
            this.gameObject.transform.position = Vector2.Lerp(this.gameObject.transform.position, new Vector2(this.gameObject.transform.position.x, 
                this.gameObject.transform.position.y + 2), MoveSpeed * Time.deltaTime);
            #endregion
        }

        yield return null;                                       //코루틴 종료
    }

    protected virtual void DestroyObject()
    {
        // 효과 넣어주면 됨

        Destroy(this.gameObject);
    }
}
