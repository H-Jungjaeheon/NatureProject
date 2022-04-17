using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("UI_변수")]
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
        //color 에 판넬 이미지 참조

        while (true)
        {
            yield return null;
            color.a += Time.deltaTime * FadeSpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            FadeImg.color = color;                                //판넬 이미지 컬러에 바뀐 알파값 참조

            if (FadeImg.color.a >= 1)                        //만약 판넬 이미지 알파 값이 0보다 작으면
            {
                break;
            }
        }

        yield return new WaitForSeconds(FadeSpeed);

        while (true)
        {
            yield return null;
            color.a -= Time.deltaTime * FadeSpeed;               //이미지 알파 값을 타임 델타 값 * 0.
            FadeImg.color = color;                                //판넬 이미지 컬러에 바뀐 알파값 참조

            if (FadeImg.color.a <= 0)                        //만약 판넬 이미지 알파 값이 0보다 작으면
            {
                break;
            }
        }
        yield return new WaitForSeconds(FadeSpeed);

        StartCoroutine(FadeInOut(StartBtn.GetComponent<Image>()));

        yield return null;                                        //코루틴 종료
    }
}
