using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [Header("UI_StartBtn����")]
    [SerializeField]
    Button StartBtn;
    [SerializeField]
    float FadeSpeed;
    [Header("UI_TitleTxt����")]
    [SerializeField]
    GameObject TitleTxt;
    [SerializeField]
    Vector2 OriginPos;
    [SerializeField]
    float MaxYPos;
    [SerializeField]
    float MoveSpeed;
    [SerializeField]
    bool OnMainScene = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInOut(StartBtn.GetComponent<Image>()));
        StartCoroutine(TitleMove());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator TitleMove()
    {
        yield return null;
        OriginPos = TitleTxt.transform.position;

        while (true)
        {
            yield return null;
            if (OnMainScene == false)
            {
                TitleTxt.transform.position = Vector2.MoveTowards(TitleTxt.transform.position, OriginPos + new Vector2(0, MaxYPos), MoveSpeed * Time.deltaTime);

                if ((OriginPos.y + MaxYPos) - 1f <= TitleTxt.transform.position.y && MaxYPos > 0)
                {
                    MaxYPos *= -1;
                }

                else if ((OriginPos.y + MaxYPos) + 1f >= TitleTxt.transform.position.y && MaxYPos < 0)
                {
                    MaxYPos *= -1;
                }
            }

            else if (OnMainScene == true)
            {

            }
        }
    }

    IEnumerator FadeInOut(Image FadeImg)
    {
        if (OnMainScene == false)
        {
            Color color = FadeImg.color;
            //color �� �ǳ� �̹��� ����

            while (true || OnMainScene == false)
            {
                yield return null;
                color.a += Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
                FadeImg.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

                if (FadeImg.color.a >= 1)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }

            yield return new WaitForSeconds(FadeSpeed);

            while (true || OnMainScene == false)
            {
                yield return null;
                color.a -= Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
                FadeImg.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

                if (FadeImg.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }
            yield return new WaitForSeconds(FadeSpeed);

            if (OnMainScene == false)
            {
                StartCoroutine(FadeInOut(StartBtn.GetComponent<Image>()));
            }

            yield return null;
        }//�ڷ�ƾ ����
    }

    public void ClickBtn()
    {
        StartCoroutine(FadeOut(StartBtn.GetComponent<Image>()));
        OnMainScene = true;
    }

    IEnumerator FadeIn(Image FadeImg)
    {
        Color color = FadeImg.color;

        while (true)
        {
            yield return null;
            color.a += Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            FadeImg.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            if (FadeImg.color.a >= 1)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
            {
                break;
            }
        }
    }

    IEnumerator FadeOut(Image FadeImg)
    {
        Color color = FadeImg.color;

        while (true)
        {
            yield return null;
            color.a -= Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
            FadeImg.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

            if (FadeImg.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
            {   
                FadeImg.gameObject.SetActive(false);
                break;
            }
        }
    }
}
