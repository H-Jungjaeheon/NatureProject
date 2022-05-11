using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SelectStageManager : MonoBehaviour
{
    [Header("스테이지 선택_씬 시작 연출 변수")]
    //0.1f ~ 1.0f
    [SerializeField]
    private GameObject[] SizeChangeObj;
    //0.05f ~ 0.3f
    public GameObject CatTower;
    [SerializeField]
    private float ChangeDur;

    [Header("스테이지 선택_청소 시작 연출 변수")]
    [SerializeField]
    GameObject CleaningTxtParant;
    [SerializeField]
    Image BlurPanel;
    [SerializeField]
    float BlurSize;
    [SerializeField]
    int MinTxtScale;
    [SerializeField]
    float TxtChangeDur;

    [Header("스테이지 선택_에러 연출 변수")]
    [SerializeField]
    Text ErrorTxt;
    [SerializeField]
    string[] ErrorComent;
    [SerializeField]
    GameObject GoalObj;
    [SerializeField]
    float ErrorTxtMoveDur;

    [Header ("스테이지 선택_변수")]
    public List<GameObject> Stages;

    public Sprite[] SwipeImage;
    int layerMask = 1 << 6;

    [SerializeField]
    string StageName;
    [SerializeField]
    int CurCatTowerPos;
    [SerializeField]
    int GoalCatTowerPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeObjSize());
        SettingStage();
        StartCoroutine(CatTowerMove());
        StartCoroutine(ChangeSizeCleaningTxt());
        StartCoroutine(PrintErrorTxt(1));
        //Debug.Log($"{GameManager.In.DicStageInfo["2_2"].Chapter} && {GameManager.In.DicStageInfo["2_2"].Stage}");
    }

    // Update is called once per frame
    void Update()
    {
        //SettingStage();

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
        }

        BlurPanel.material.SetFloat("_Radius", BlurSize);
    }

    private void MouseClick()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(Pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1.0f, layerMask);

        if (hit.collider != null)
        {
            StageName = hit.collider.gameObject.name;
            Debug.Log(hit.collider.gameObject.name);
            GoalSelectStage();
        }
    }

    public void StagePlay()
    {
        if (GameManager.In.DicStageInfo[StageName].Onplay == true)
        {
            GameManager.In.Energy -= GameManager.In.DicStageInfo[StageName].SpendEnergy;
            GameManager.In.DicStageInfo[StageName].ClearCheck = true;
        }
    }

    #region SelectStage함수
    void CurSelectStage()
    {
        string[] Names = StageName.Split('_');
        CurCatTowerPos = int.Parse(Names[1]) - 1;
    }

    void GoalSelectStage()
    {
        if (GameManager.In.DicStageInfo[StageName].Onplay == true)
        {
            string[] Names = StageName.Split('_');
            GoalCatTowerPos = int.Parse(Names[1]) - 1;
        }
    }
    #endregion

    void SettingStage()
    {
        StageName = Stages[0].name;
        CurSelectStage();
        GoalSelectStage();

        //CatTower.transform.position = 

        foreach (GameObject Stage in Stages)
        {
            if(GameManager.In.DicStageInfo[Stage.name].Onplay == true)
            {
                Stage.gameObject.GetComponent<SpriteRenderer>().sprite = SwipeImage[1];

                if (GameManager.In.DicStageInfo[Stage.name].ClearCheck == true)
                {
                    Stage.gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    string[] StageNameArr = Stage.name.Split('_');
                    string StageName = $"{StageNameArr[0]}_{int.Parse(StageNameArr[1]) + 1}".ToString();

                    if (GameManager.In.DicStageInfo.ContainsKey(StageName) == true)
                    {
                        GameManager.In.DicStageInfo[StageName].Onplay = true;
                    }

                    else if (GameManager.In.DicStageInfo.ContainsKey(StageName) == false)
                    {
                        GameManager.In.UnlockStage = int.Parse(StageNameArr[0]) + 1;

                        StageName = $"{int.Parse(StageNameArr[0]) + 1}_{1}".ToString();
                        GameManager.In.DicStageInfo[StageName].Onplay = true;
                    }
                }

                else
                {
                    Stage.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            else
            {
                Stage.gameObject.GetComponent<SpriteRenderer>().sprite = SwipeImage[0];
                Stage.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            
            //Debug.Log($"Chapter{StageInfo[0]}, Stage{StageInfo[1]}");

            //if(GameManager)
        }
    }

    IEnumerator CatTowerMove()
    {
        yield return null;

        while(true)
        {
            yield return null;

            if(CurCatTowerPos != GoalCatTowerPos)
            {
                if(CurCatTowerPos < GoalCatTowerPos)
                {
                    CatTower.transform.position = Vector2.MoveTowards(CatTower.transform.position, Stages[CurCatTowerPos + 1].transform.position + new Vector3(0 , 1.5f, 0) , 7 * Time.deltaTime);

                    if(CatTower.transform.position.x == Stages[CurCatTowerPos + 1].transform.position.x)
                        CurCatTowerPos++;

                }

                else if (CurCatTowerPos > GoalCatTowerPos)
                {
                    CatTower.transform.position = Vector2.MoveTowards(CatTower.transform.position, Stages[CurCatTowerPos - 1].transform.position + new Vector3(0, 1.5f, 0), 7 * Time.deltaTime);
                    if (CatTower.transform.position.x == Stages[CurCatTowerPos - 1].transform.position.x)
                        CurCatTowerPos--;
                }
            }
        }
    }

    IEnumerator ChangeObjSize()
    {
        yield return null;

        foreach(GameObject Obj in SizeChangeObj)
        {
            Obj.transform.DOScale(1.0f, ChangeDur); 
        }

        CatTower.transform.DOScale(0.3f, ChangeDur);
    }

    IEnumerator ChangeSizeCleaningTxt()
    {
        yield return null;

        DOTween.To(() => BlurSize, x => BlurSize = x, 1, TxtChangeDur);
        CleaningTxtParant.transform.DOScale(MinTxtScale, TxtChangeDur);
        Debug.Log(BlurPanel.material.GetFloat("_Radius"));

        //페이드인/아웃 넣을 곳
    }

    IEnumerator PrintErrorTxt(int ErrorType)
    {
        yield return null;

        ErrorTxt.text = ErrorComent[ErrorType];

        ErrorTxt.gameObject.transform.DOMove(GoalObj.transform.position, ErrorTxtMoveDur);
        //페이드인/아웃 넣을 곳
    }
}
