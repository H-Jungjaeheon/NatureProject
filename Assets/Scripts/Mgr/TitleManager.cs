using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("UI_����")]
    [SerializeField]
    Button StartBtn; 
    [SerializeField]
    float FadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInOut(StartBtn.GetComponent<Image>()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeInOut(Image FadeImg)
    {
        Color color = FadeImg.color;
        //color �� �ǳ� �̹��� ����

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

        yield return new WaitForSeconds(FadeSpeed);

        while (true)
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

        StartCoroutine(FadeInOut(StartBtn.GetComponent<Image>()));

        yield return null;                                        //�ڷ�ƾ ����
    }
}
