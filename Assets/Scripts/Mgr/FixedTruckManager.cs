using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FixedTruckManager : MonoBehaviour
{
    #region MainManager..
    [Header("Gizmo - 소환 영역 표시")]
    [SerializeField]
    Vector2 SpawnPos;

    [SerializeField]
    float SizeX;
    [SerializeField]
    float SizeY;

    [Header("실제영역계산(실제스폰영역 확인)")] // 에디터에서 건드리지 않아도 됌
    [SerializeField]
    GameObject Boundary;

    [SerializeField]
    float SpawnPosX, SpawnPosY;

    [Header("UI_변수")]
    [SerializeField]
    Text EnergyTxt;
    [SerializeField]
    Text FoodTxt;
    [SerializeField]
    Text MoneyTxt;

    [Header("ButtonSlide_변수")]
    [SerializeField]
    GameObject ParantButtons;
    [SerializeField]
    int CurButton = 1;
    [SerializeField]
    List<Vector2> MovePoint;
    [SerializeField]
    List<RawImage> Buttons;
    [SerializeField]
    float MoveSpeed;
    [SerializeField]
    Vector2 BeginPos1, EndPos2;
    [SerializeField]
    bool OnSlide = false;
    #endregion

    #region TruckScene

    [Header("TruckSceneUI_변수")]
    [SerializeField]
    Text HPCostText;
    [SerializeField]
    Text ChargeCostText;
    [SerializeField]
    Text PowerCostText;

    [Space(10)]

    [SerializeField]
    Text HPLevelText;
    [SerializeField]
    Text ChargeLevelText;
    [SerializeField]
    Text PowerLevelText;

    [Space(10)]

    [SerializeField]
    Text HpInfoText;
    [SerializeField]
    Text ChargeInfoText;
    [SerializeField]
    Text PowerInfoText;


    [Header("TruckUpgreadCost_변수")]
    [SerializeField]
    int BodyCost;
    [SerializeField]
    int ChargeCost;
    [SerializeField]
    int PowerCost;


    [Header("TruckScene연출_변수")]
    [SerializeField]
    List<Vector3> MoveCamPos;
    [SerializeField]
    List<RawImage> checkImgs;
    [SerializeField]
    Texture[] SwipeImg;

    #endregion

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        UpdateSetting();
        Mouse();
        SlideButtons();
        ActiveButton();
    }

    //#region 쓰레기 치우기
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(SpawnPos, new Vector2(SizeX, SizeY));

    //    Boundary.transform.position = SpawnPos;
    //    Boundary.transform.localScale = new Vector2(SizeX, SizeY);

    //    SpawnPosX = Boundary.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x;
    //    SpawnPosY = Boundary.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y;
    //}

    //#endregion

    private void StartSetting()
    {
        TextSetting();
    }

    private void UpdateSetting()
    {
        EnergyTxt.text = $"{GameManager.In.Energy}/{GameManager.In.MaxEnergy}";
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}";

        TextSetting();
    }

    #region UpgreadSetting
    private void TextSetting()
    {
        UpgreadCostSetting();
        LevelTxtSetting();
        CostTxtSetting();
        InfoTxtSetting();
    }

    private void UpgreadCostSetting()
    {
        BodyCost = 500 + (100 * (GameManager.In.BodyLevel - 1));
        ChargeCost = 400 + (100 * (GameManager.In.LaserChargeLevel - 1));
        PowerCost = 600 + (100 * (GameManager.In.LaserPowerLevel - 1));
    }

    private void LevelTxtSetting()
    {
        string Max = "MAX";

        HPLevelText.text = $"트럭 몸체 Lv.{(GameManager.In.BodyLevel >= GameManager.In.MaxLevel ? Max : GameManager.In.BodyLevel.ToString())}";
        ChargeLevelText.text = $"태양열 발전기 Lv.{(GameManager.In.LaserChargeLevel >= GameManager.In.MaxLevel ? Max : GameManager.In.LaserChargeLevel.ToString())}";
        PowerLevelText.text = $"레이저 빔 발사기 Lv.{(GameManager.In.LaserPowerLevel >= GameManager.In.MaxLevel ? Max : GameManager.In.LaserPowerLevel.ToString())}";
    }

    private void CostTxtSetting()
    {
        HPCostText.text = $"{BodyCost}원";
        ChargeCostText.text = $"{ChargeCost}원";
        PowerCostText.text = $"{PowerCost}원";
    }

    private void InfoTxtSetting()
    {
        HpInfoText.text = $"{2000 + (100 * (GameManager.In.BodyLevel - 1))}";
        ChargeInfoText.text = $"{180 - (2 * (GameManager.In.LaserChargeLevel - 1))}s";
        PowerInfoText.text = $"{100 + (20 * (GameManager.In.LaserPowerLevel - 1))}";
    }
    #endregion

    private void Mouse()
    {
        int idx = 0;

        if (Input.GetMouseButtonDown(0))
        {
            OnSlide = true;
            Debug.Log(MovePoint.Count);
            BeginPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            EndPos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (BeginPos1.x > EndPos2.x + 5 && OnSlide == true)
            {
                Debug.Log("++");
                OnSlide = false;

                if (CurButton < MovePoint.Count - 1)
                {
                    CurButton += 1;
                }
                //TrueSlideEgg(1);
            }

            else if (BeginPos1.x < EndPos2.x - 5 && OnSlide == true)
            {
                Debug.Log("--");
                OnSlide = false;

                if (CurButton > 0)
                {
                    CurButton += -1;
                }
                //TrueSlideEgg(-1);
            }

            foreach (RawImage image in Buttons)
            {
                if (idx == CurButton)
                {
                    image.raycastTarget = true;
                    image.gameObject.GetComponent<Button>().interactable = true;
                }

                else
                {
                    image.raycastTarget = false;
                    image.gameObject.GetComponent<Button>().interactable = false;
                }

                idx++;
            }
        }

    }

    private void SlideButtons()
    {
        ParantButtons.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ParantButtons.GetComponent<RectTransform>().anchoredPosition, MovePoint[CurButton], MoveSpeed * Time.deltaTime);
    }

    private void ActiveButton()
    {
        int idx = 0;

        Camera cam = Camera.main;
        cam.gameObject.transform.position = Vector3.Lerp(cam.gameObject.transform.position, MoveCamPos[CurButton], MoveSpeed * Time.deltaTime);

        foreach (RawImage image in checkImgs)
        {
            if (idx == CurButton)
            {
                image.texture = SwipeImg[1];
            }

            else
            {
                image.texture = SwipeImg[0];
            }

            idx++;
        }
    }

    #region 레이저 업그레이드 함수
    public void LevelUpBody()
    {
        if (GameManager.In.MaxLevel > GameManager.In.BodyLevel)
        {
            GameManager.In.Money -= BodyCost;
            GameManager.In.BodyLevel++;
        }
    }

    public void LevelUpLaserCharge()
    {
        if (GameManager.In.MaxLevel > GameManager.In.LaserChargeLevel)
        {
            GameManager.In.Money -= ChargeCost;
            GameManager.In.LaserChargeLevel++;
        }
    }

    public void LevelUpLaserPower()
    {
        if (GameManager.In.MaxLevel > GameManager.In.LaserPowerLevel)
        {
            GameManager.In.Money -= PowerCost;
            GameManager.In.LaserPowerLevel++;
        }
    }
    #endregion
}
