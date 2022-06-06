using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class LottoryManager : MonoBehaviour
{
    #region �˽����̵�
    [Header("�˽����̵�_����")]
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
    #endregion
    #region ui
    [Header("UI_����")]
    [SerializeField]
    Text EnergyTxt;
    [SerializeField]
    Text FoodTxt;
    [SerializeField]
    Text MoneyTxt;
    [SerializeField]
    Text BuyMoneyTxt;
    #endregion
    #region Egg�̱�
    [Header("Egg�̱�_����")]
    [SerializeField]
    private int LottoryCost;
    [SerializeField]
    GameObject FadeImg;
    [SerializeField]
    float FadeSpeed;
    #endregion
    #region Egg�̱�Ȯ��
    [Header("Egg�̱�Ȯ��_����")]
    [SerializeField]
    float MaxPersent;

    [SerializeField]
    LottoryPersent SelectLottoryType;
    #endregion
    #region Egg�̱�ui
    [Header("Egg�̱�UI_����")]
    [SerializeField]
    GameObject Info_Pan;
    [SerializeField]
    RawImage Info_Img;
    [SerializeField]
    Text Info_Name;
    [SerializeField]
    Text Info_Cost;
    [SerializeField]
    GameObject Info_Fade;
    [SerializeField]
    bool OnInfo = false;

    #endregion
    #region Shake�Լ�
    [Header("Shake�Լ�_����")]
    [SerializeField]
    Vector3 OrizinPos;
    [SerializeField]
    float BasicShakePower;
    [SerializeField]
    float CurShakePower;
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
    #endregion

    [Header("���̵�_����")]
    [SerializeField]
    private List<Button> SceneChangeBtns;

    private void Start()
    {
        SettingEggPos();
        AddChangeSceneBtn();
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
        EnergyTxt.text = $"{GameManager.In.Energy}/{GameManager.In.MaxEnergy}";
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}";
        BuyMoneyTxt.text = $"{MainEgg.GetComponent<Egg>().BuyMoney.ToString("N0")}";
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
                LottorySetting();
                LottoryCost = data.EggType.GetComponent<Egg>().BuyMoney;
            }
        }
    }

    private void SettingEggPos()
    {
        int Idx = 0;
        CurShakePower = BasicShakePower;

        foreach (Transform Tr in EggZip.transform)
        {
            EggData eggData = new EggData();

            eggData.EggType = Tr.gameObject;

            if (Idx == 0)
            {
                MainEgg = eggData.EggType;
                LottorySetting();
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
            StartLottory();
        }
    }

    public void LottorySetting()
    {
        MaxPersent = 0;

        foreach(LottoryPersent Persents in MainEgg.GetComponent<Egg>().lottoryPersents)
        {
            MaxPersent += Persents.Persent;
        }
    }

    public void StartLottory()
    {
        int RanPersent = (int)UnityEngine.Random.Range(0, MaxPersent + 1);
        //int RanPersent = 100; //�׽�Ʈ ��

        foreach (LottoryPersent Persents in MainEgg.GetComponent<Egg>().lottoryPersents)
        {
            RanPersent -= Persents.Persent;

            if(RanPersent <= 0)
            {
                SelectLottoryType = Persents;
                Debug.Log(Persents.Name);
                break;
            }
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
            MainEgg.transform.GetChild(0).transform.DORotate(OrizinPos + new Vector3(0, 0, MoveMaximum), CurShakePower);

            Timer += Time.deltaTime;

            if (CurShakePower >= MaximumShakePower)
            {
                if (ShakeDuration / 2 >= Timer)
                {
                    CurShakePower -= Time.deltaTime * MinusShakePower;
                }

                else if (ShakeDuration / 2 < Timer)
                {
                    CurShakePower -= Time.deltaTime * (MinusShakePower * 2);
                }
            }
        }

        FadeImg.SetActive(true);
        StartCoroutine(FadeInOut(FadeImg.GetComponent<RawImage>()));

        CurShakePower = BasicShakePower;
        Debug.Log("End");
    }

    public void StartOnInfo()
    {
        StartCoroutine(OnInfoPan());
    }

    public IEnumerator OnInfoPan()
    {
        yield return null;

        if (OnInfo == false)
        {
            OnInfo = true;
            Info_Img.texture = SelectLottoryType.UnitImg; //���ҽ� ������ �Է�
            Info_Img.transform.localScale = new Vector2(SelectLottoryType.ImgScale, SelectLottoryType.ImgScale);
            Info_Img.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, SelectLottoryType.ImgPosition);

            Info_Name.text = SelectLottoryType.Name;
            Info_Cost.text = $"{SelectLottoryType.SpawnCost}��"; //���ҽ� ������ �Է�

            Info_Fade.SetActive(true);

            Color color = Info_Fade.GetComponent<RawImage>().color;
            //color �� �ǳ� �̹��� ����

            while (true)
            {
                yield return null;
                color.a += Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
                Info_Fade.GetComponent<RawImage>().color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

                if (Info_Fade.GetComponent<RawImage>().color.a >= 0.5f)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }

            Info_Pan.SetActive(true);

            while (true)
            {
                Debug.Log("qwe");
                yield return null;
                Info_Pan.transform.DOScale(Vector2.one, FadeSpeed);

                if (Info_Pan.transform.localScale.x >= 0.99f)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }
        }

        else if (OnInfo == true)
        {
            OnInfo = false;
            Color color = Info_Fade.GetComponent<RawImage>().color;

            while (true)
            {
                Debug.Log("ui");
                yield return null;
                Info_Pan.transform.DOScale(new Vector2(0,0), FadeSpeed);

                if (Info_Pan.transform.localScale.x <= 0.02f)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }

            Info_Pan.SetActive(false);

            while (true)
            {
                yield return null;
                color.a -= Time.deltaTime * FadeSpeed;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.
                Info_Fade.GetComponent<RawImage>().color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����

                if (Info_Fade.GetComponent<RawImage>().color.a <= 0.0f)                        //���� �ǳ� �̹��� ���� ���� 0���� ������
                {
                    break;
                }
            }
            Info_Fade.SetActive(false);

            isShake = false;
        }
    }

    IEnumerator FadeInOut(RawImage FadeImg)
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

        MainEgg.transform.GetChild(0).rotation = Quaternion.Euler(OrizinPos);
        yield return new WaitForSeconds(0.5f);

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

        FadeImg.gameObject.SetActive(false);
        StartOnInfo();

        yield return null;                                        //�ڷ�ƾ ����
    }

    void AddChangeSceneBtn()
    {
        for (int i = 0; i < SceneChangeBtns.Count; i++)
        {
            int temp = i;

            SceneChangeBtns[temp].onClick.AddListener(() =>
            {
                StartCoroutine(SceneChange(SceneChangeBtns[temp].GetComponent<SceneChangeBtn>().SceneName));
                Debug.Log(SceneChangeBtns[temp].GetComponent<SceneChangeBtn>().SceneName);
            });
        }
    }

    IEnumerator SceneChange(string SceneName)
    {
        yield return null;

        Debug.Log("qwe");
        SceneManager.LoadScene(SceneName);
    }
}

