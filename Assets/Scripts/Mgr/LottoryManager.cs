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

    private void Start()
    {
        SettingEggPos();
    }

    private void Update()
    {
        MouseDrag();

        SettingTxt();

        SlideEggs();
    }

    private void SettingTxt()
    {
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}개";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}원";
        BuyMoneyTxt.text = $"{MainEgg.GetComponent<Egg>().BuyMoney.ToString("N0")})";
    }

    private void MouseDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            BeginPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnSlide = true;
        }

        if(Input.GetMouseButton(0))
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

    private void TrueSlideEgg(int Dir)
    {
        foreach (EggData data in Eggs)
        {
            data.NextPos += Dir;

            if(data.NextPos > EggZip.transform.childCount - 1)
            {
                data.NextPos = 0;
            }

            else if(data.NextPos < 0)
            {
                data.NextPos = EggZip.transform.childCount - 1;
            }

            if(data.NextPos == 0)
            {
                MainEgg = data.EggType;
            }
        }
    }

    private void SettingEggPos()
    {
        int Idx = 0;

        foreach(Transform Tr in EggZip.transform)
        {
            EggData eggData = new EggData();

            eggData.EggType = Tr.gameObject;

            if (Idx == 0)
            {
                MainEgg = eggData.EggType;
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
}
