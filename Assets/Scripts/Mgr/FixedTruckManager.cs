using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FixedTruckManager : MonoBehaviour
{ 
    #region MainManager..
    [Header("Gizmo - ��ȯ ���� ǥ��")]
    [SerializeField]
    Vector2 SpawnPos;

    [SerializeField]
    float SizeX;
    [SerializeField]
    float SizeY;

    [Header("�����������(������������ Ȯ��)")] // �����Ϳ��� �ǵ帮�� �ʾƵ� ��
    [SerializeField]
    GameObject Boundary;

    [SerializeField]
    float SpawnPosX, SpawnPosY;

    [Header("UI_����")]
    [SerializeField]
    Text EnergyTxt;
    [SerializeField]
    Text FoodTxt;
    [SerializeField]
    Text MoneyTxt;

    [Header("ButtonSlide_����")]
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

    [Header("TruckSceneUI_����")]
    [SerializeField]
    List<Text> UpgradeText;

    [Header("TruckScene����_����")]
    [SerializeField]
    List<Vector3> MoveCamPos;
    [SerializeField]
    List<RawImage> checkImgs;

    #endregion

    private void Start()
    {

    }

    private void Update()
    {
        SettingTxt();
        Mouse();
        SlideButtons();
        ActiveButton();
    }

    //#region ������ ġ���
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

    private void SettingTxt()
    {
        EnergyTxt.text = $"{GameManager.In.Energy}/{GameManager.In.MaxEnergy}";
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}��";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}��";
    }

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

                if(CurButton < MovePoint.Count-1)
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

        foreach(RawImage image in checkImgs)
        {
            if(idx == CurButton)
            {
                image.color = Color.red;
            }

            else
            {
                image.color = Color.white;
            }

            idx++;
        }
    }

    #region ������ ���׷��̵� �Լ�
    public void LevelUpBody()
    {

    }

    public void LevelUpLaserCharge()
    {

    }

    public void LevelUpLaserPower()
    {

    }
    #endregion
}
