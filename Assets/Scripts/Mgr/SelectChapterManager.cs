using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChapterManager : MonoBehaviour
{
    [Header("챕터 선택_변수")]
    [SerializeField]
    private Button[] Chapters; 
    [SerializeReference]
    public Sprite[] OnSwipeImage; 
    public Sprite[] OffSwipeImage; 

    // Update is called once per frame
    void Update()
    {
        ActiveButton();
    }

    private void ActiveButton()
    {
        int idx = 0;

        foreach (Button Btn in Chapters)
        {
            if (idx < GameManager.In.UnlockStage)
            {
                Btn.GetComponent<Image>().sprite = OnSwipeImage[idx];
                Btn.GetComponent<Image>().raycastTarget = true;
            }

            else
            {
                Btn.GetComponent<Image>().sprite = OffSwipeImage[idx];
                Btn.GetComponent<Image>().raycastTarget = false;
            }

            idx++;
        }
    }
}
