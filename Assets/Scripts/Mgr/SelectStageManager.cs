using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageManager : MonoBehaviour
{
    public List<GameObject> Stages;
    public Sprite[] SwipeImage;
    int layerMask = 1 << 6;
    [SerializeField]
    string StageName;

    // Start is called before the first frame update
    void Start()
    {
        SettingStage();
        //Debug.Log($"{GameManager.In.DicStageInfo["2_2"].Chapter} && {GameManager.In.DicStageInfo["2_2"].Stage}");
    }

    // Update is called once per frame
    void Update()
    {
        SettingStage();

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
        }

        Debug.Log(GameManager.In.DicStageInfo.ContainsKey("1_6"));
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

    void SettingStage()
    {
        foreach(GameObject Stage in Stages)
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
}
