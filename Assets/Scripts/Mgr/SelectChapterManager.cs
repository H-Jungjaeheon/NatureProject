using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SelectChapterManager : MonoBehaviour
{
    [Header("챕터 선택_변수")]
    [SerializeField]
    private Button[] Chapters;
    [SerializeReference]
    public Sprite[] OnSwipeImage; 
    public Sprite[] OffSwipeImage;
    [SerializeField]
    public GameObject Map;

    [Header("챕터 선택_연출변수")]
    [SerializeField]
    private Camera Cam;
    [SerializeField]
    private int CameraSize;
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private Vector3[] CameraMovePos;
    [Space(5)]
    [SerializeField]
    private float CamMoveDur;
    [SerializeField]
    private float CamMoveSize;

    [Header("씬이동_변수")]
    [SerializeField]
    private List<Button> SceneChangeBtns;

    // Update is called once per frame
    void Update()
    {
        ActiveButton();
        AddChangeSceneBtn();
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

    public void SelectChapter1()
    {
        StartCoroutine(CameraMove(0));
    }

    public void SelectChapter2()
    {
        StartCoroutine(CameraMove(1));
    }

    public void SelectChapter3()
    {
        StartCoroutine(CameraMove(2));
    }

    private IEnumerator CameraMove(int Chapter)
    {
        yield return null;
        UI.gameObject.SetActive(false);
        DOTween.To(() => Cam.orthographicSize, x => Cam.orthographicSize = x, CamMoveSize, CamMoveDur);
        Map.transform.DOScale(Vector3.one * 0.7f, CamMoveDur);
        Cam.transform.DOMove(CameraMovePos[Chapter], CamMoveDur);

        yield return new WaitForSeconds(CamMoveDur);

        SceneManager.LoadScene($"Stage{Chapter + 1}Scene");
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
