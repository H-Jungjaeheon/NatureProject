using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageManager : MonoBehaviour
{
    public List<GameObject> Stages;
    public Sprite[] SwipeImage;
    int layerMask = 1 << 6;

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
    }

    private void MouseClick()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(Pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1.0f, layerMask);

        if (hit.collider != null)
        {
            Debug.Log("sad");
        }
    }

    void SettingStage()
    {
        foreach(GameObject Stage in Stages)
        {
            if(GameManager.In.DicStageInfo[Stage.name].Onplay == true)
            {
                Stage.gameObject.GetComponent<SpriteRenderer>().sprite = SwipeImage[1];
            }

            else
            {
                Stage.gameObject.GetComponent<SpriteRenderer>().sprite = SwipeImage[0];
            }
            
            //Debug.Log($"Chapter{StageInfo[0]}, Stage{StageInfo[1]}");

            //if(GameManager)
        }
    }
}
