using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LottoryManager : MonoBehaviour
{
    [Header("알슬라이드_변수")]
    [SerializeField]
    GameObject EggZip;
    [SerializeField]
    List<Vector3> EggPos;
    [SerializeField]
    List<Vector3> EggScale;
    [SerializeField]
    List<EggData> Eggs;
    [SerializeField]
    GameObject MainEgg;
    [SerializeField]
    float MoveSpeed;
    [SerializeField]
    Vector2 BeginPos1, EndPos2;
    [SerializeField]
    bool OnSlide = false;

    [Header("UI_변수")]
    [SerializeField]
    Text EnergyTxt;
    [SerializeField]
    Text FoodTxt;
    [SerializeField]
    Text MoneyTxt;
    [SerializeField]
    Text BuyMoneyTxt;

    [Header("Egg뽑기_변수")]
    [SerializeField]
    private int LottoryCost;
    [SerializeField]
    GameObject FadeImg;
    [SerializeField]
    float FadeSpeed;

    [Header("Shake함수_변수")]
    [SerializeField]
    Vector3 OrizinPos;
    [SerializeField]
    float BasicShakePower;
    [SerializeField]
    float MinusShakePower;
    [SerializeField]
    float MoveMaximum;
    [SerializeField]
    float MaximumShakePower;
    [SerializeField]
    float ShakeDuration;
    [SerializeField]
    bool isShake = false;

    private void Start()
    {
        SettingEggPos();
    }

    private void Update()
    {
        Mouse();

        BasicSetting();

        SlideEggs();
    }

    private void BasicSetting()
    {
        OrizinPos = new Vector3(0, 0, 0);
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}개";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}원";
        BuyMoneyTxt.text = $"{MainEgg.GetComponent<Egg>().BuyMoney.ToString("N0")})";
    }

    private void Mouse()
    {
        if (isShake == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeginPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                OnSlide = true;
            }

            if (Input.GetMouseButton(0))
            {
                EndPos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (BeginPos1.x > EndPos2.x + 5 && OnSlide == true)
                {
                    OnSlide = false;

                    TrueSlideEgg(1);
                }

                else if (BeginPos1.x < EndPos2.x - 5 && OnSlide == true)
                {
                    OnSlide = false;

                    TrueSlideEgg(-1);
                }
            }
        }
    }

    #region EggSlide
    private void TrueSlideEgg(int Dir)
    {
        foreach (EggData data in Eggs)
        {
            data.NextPos += Dir;

            if (data.NextPos > EggZip.transform.childCount - 1)
            {
                data.NextPos = 0;
            }

            else if (data.NextPos < 0)
            {
                data.NextPos = EggZip.transform.childCount - 1;
            }

            if (data.NextPos == 0)
            {
                MainEgg = data.EggType;
                LottoryCost = data.EggType.GetComponent<Egg>().BuyMoney;
            }
        }
    }

    private void SettingEggPos()
    {
        int Idx = 0;

        foreach (Transform Tr in EggZip.transform)
        {
            EggData eggData = new EggData();

            eggData.EggType = Tr.gameObject;

            if (Idx == 0)
            {
                MainEgg = eggData.EggType;
                LottoryCost = eggData.EggType.GetComponent<Egg>().BuyMoney;
            }

            else
            {
                eggData.NextPos = Idx;
            }

            Eggs.Add(eggData);

            EggPos.Add(Tr.position);
            EggScale.Add(Tr.localScale);

            Idx++;
        }
    }

    private void SlideEggs()
    {
        int idx = 0;

        foreach (EggData data in Eggs)
        {
            data.EggType.transform.DOMove(EggPos[data.NextPos], MoveSpeed);
            //data.EggType.transform.position = Vector2.Lerp(data.EggType.transform.position, EggPos[data.NextPos], MoveSpeed * Time.deltaTime);

            data.EggType.transform.DOScale(EggScale[data.NextPos], MoveSpeed);

            //if((System.Math.Round(data.EggType.transform.position.x * 10) / 10).Equals(EggPos[data.NextPos]))
            //{
            //    Debug.Log("asd");
            //}

            idx++;
        }
    }
    #endregion

    public void BuyClickEvent()
    {
        if (isShake == false)
        {
            GameManager.In.Money -= LottoryCost;
            isShake = true;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        yield return null;
        float Timer = 0.0f;
        float ChangeMoveDir = -1.0f;

        while (ShakeDuration >= Timer)
        {
            if (MoveMaximum > 0 && (Math.Round(MainEgg.transform.GetChild(0).transform.localEulerAngles.z * 10) / 10).Equals(MoveMaximum))
            {
                MoveMaximum *= -1;
            }

            else if (MoveMaximum < 0 && (Math.Round(MainEgg.transform.GetChild(0).transform.localEulerAngles.z * 10) / 10).Equals(360 + MoveMaximum))
            {
                MoveMaximum *= -1;
            }

            yield return null;
            MainEgg.transform.GetChild(0).transform.DORotate(OrizinPos + new Vector3(0, 0, MoveMaximum), BasicShakePower);

            Timer += Time.deltaTime;

            if (BasicShakePower >= MaximumShakePower)
            {
                if (ShakeDuration / 2 >= Timer)
                {
                    Debug.Log(MinusShakePower);
                    BasicShakePower -= Time.deltaTime * MinusShakePower;
                }

                else if (ShakeDuration / 2 < Timer)
                {
                    Debug.Log(MinusShakePower * 2);
                    BasicShakePower -= Time.deltaTime * (MinusShakePower * 2);
                }
            }
        }

        FadeImg.SetActive(true);
        StartCoroutine(FadeInOut(FadeImg.GetComponent<RawImage>()));

        isShake = false;
        Debug.Log("End");
    }

    IEnumerator FadeInOut(RawImage FadeImg)
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

        MainEgg.transform.GetChild(0).rotation = Quaternion.Euler(OrizinPos);
        yield return new WaitForSeconds(0.5f);

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

        FadeImg.gameObject.SetActive(false);

        yield return null;                                        //코루틴 종료
    }
}

