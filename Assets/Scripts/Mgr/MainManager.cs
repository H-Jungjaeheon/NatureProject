using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    #region 쓰레기 소환
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

    [Header("소환 변수")]
    [SerializeField]
    GameObject Trash;
    [SerializeField]
    GameObject Brush;
    [SerializeField]
    int MaxSpawnCnt;
    [SerializeField] // 레이어 마스크(체크용)
    int layerMask = 1 << 6;
    #endregion

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

    private void Start()
    {
        SpawnTrash();
    }

    private void Update()
    {
        SettingTxt();

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
        }

        Mouse();
        SlideButtons();
    }

    #region 쓰레기 치우기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SpawnPos, new Vector2(SizeX, SizeY));

        Boundary.transform.position = SpawnPos;
        Boundary.transform.localScale = new Vector2(SizeX, SizeY);

        SpawnPosX = Boundary.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x;
        SpawnPosY = Boundary.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y;
    }

    private void MouseClick()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(Pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 5.0f, layerMask);

        if (hit.collider != null)
        {
            Debug.Log("asd");
            Instantiate(Brush, hit.collider.gameObject.transform.position, Quaternion.identity);
            hit.collider.gameObject.GetComponent<Main_Trash>().OnHit();
        }
    }

    private void SpawnTrash()
    {
        for (int i = 0; i < MaxSpawnCnt + Random.Range(-1, 1); i++)
        {
            float RanPosX = Random.Range(-SpawnPosX, SpawnPosX + 0.1f);
            float RanPosY = Random.Range(-SpawnPosY, SpawnPosY + 0.1f);

            GameObject TrashObj = Instantiate(Trash, new Vector2(RanPosX, RanPosY), Quaternion.identity);
        }
    }
    #endregion

    private void SettingTxt()
    {
        EnergyTxt.text = $"{GameManager.In.Energy}/{GameManager.In.MaxEnergy}";
        FoodTxt.text = $"{GameManager.In.Food.ToString("N0")}";
        MoneyTxt.text = $"{GameManager.In.Money.ToString("N0")}";
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
}
