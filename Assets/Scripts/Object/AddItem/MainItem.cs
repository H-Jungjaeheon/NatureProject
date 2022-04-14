using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class MainItem : MonoBehaviour
{
    [Header("���� ������ ��� ����")]
    [SerializeField]
    protected int AddCount;
    [SerializeField]
    protected TextMeshProUGUI AddCountText;
    [SerializeField]
    protected TextMeshProUGUI PlusText;
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

    protected IEnumerator FadeOut(SpriteRenderer Image,  TextMeshProUGUI TMP_Plus, TextMeshProUGUI TMP_Count)
    {
        Color Imgcolor = Image.color;                            //color �� �ǳ� �̹��� ����
        Color PlusTxtcolor = TMP_Plus.color;                     //color �� �ǳ� �̹��� ����
        Color CountTxtcolor = TMP_Count.color;                   //color �� �ǳ� �̹��� ����

        while (true)
        {
            #region ���İ� ����
            yield return null;

            Imgcolor.a -= Time.deltaTime * DestroySpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            Image.color = Imgcolor;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            PlusTxtcolor.a -= Time.deltaTime * DestroySpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            TMP_Plus.color = PlusTxtcolor;

            CountTxtcolor.a -= Time.deltaTime * DestroySpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            TMP_Count.color = CountTxtcolor;

            if (Image.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
            {
                DestroyObject();
                break;
            }
            #endregion

            #region ������Ʈ �̵�
            this.gameObject.transform.position = Vector2.Lerp(this.gameObject.transform.position, new Vector2(this.gameObject.transform.position.x, 
                this.gameObject.transform.position.y + 2), MoveSpeed * Time.deltaTime);
            #endregion
        }

        yield return null;                                       //�ڷ�ƾ ����
    }

    protected virtual void DestroyObject()
    {
        // ȿ�� �־��ָ� ��

        Destroy(this.gameObject);
    }
}