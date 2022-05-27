using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FormingManager : MonoBehaviour
{
    [Header("편성 박스 추가")]
    [SerializeField]
    private ScrollRect ScrollView; // 스크롤 뷰 위치
    [SerializeField]
    private GameObject Contents; // 스크롤 뷰 속 콘텐츠 위치
    [SerializeField]
    private GameObject FormingBox; // 추가 할 포밍 오브젝트

    [SerializeField]
    private List<GameObject> CharacterList; // 추가된 포밍 박스 리스트


    [SerializeField]
    private GameObject CharacterImgBox; // 위에 편성한 유닛 이미지
    [SerializeField]
    private List<GameObject> FormingBoxs; // 편성 박스 위치

    public GameUnitData SelectFormingUnit; // 현재 선택한 유닛 데이터

    [Header("Clone박스 생성")]
    [SerializeField]
    private GameObject CloneBoxData; //선택 하고 드래그 했을 때 생길 CloneBoxData
    private GameObject CloneBox; //선택 하고 드래그 했을 때 생길 CloneBox

    [SerializeField]
    private Vector2 StartMB_Pos; //마우스 시작 위치
    [SerializeField]
    private Vector2 CurMB_Pos; // 마우스 현재 위치
    [SerializeField]
    private bool SetClone = false; // 클론이 생성 됬는지 확인

    // Start is called before the first frame update
    void Start()
    {
        SceneSetting();
    }

    // Update is called once per frame
    void Update()
    {
        ObjectDrag();
    }

    void SceneSetting() //포밍 씬 셋팅 함수
    {
        LoadCharacter();
    }

    void LoadCharacter() // 포밍 박스 추가 함수
    {
        GameObject FormingCharacterImg = null;
        Text FormingCharacterName = null;
        Text FormingCharacterCost = null;

        foreach (GameUnitData Data in GameManager.In.GameUnitData) //게임 매니져 속 유닛 데이터 만큼 반복
        {
            GameObject FormingType = Instantiate(FormingBox);
            FormingType.GetComponent<FormingBoxData>().BoxData = Data;//포밍 박스 데이터 삽입

            FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            FormingType.transform.parent = Contents.transform;
            FormingType.transform.localScale = Vector3.one; //포밍 박스 게임 속 위치 조정

            FormingCharacterImg = FormingType.transform.GetChild(0).transform.GetChild(0).gameObject; //포밍 캐릭터 이미지 위치 지정
            FormingCharacterName = FormingType.transform.GetChild(1).gameObject.GetComponent<Text>(); //포밍 캐릭터 이름 위치 지정
            FormingCharacterCost = FormingType.transform.GetChild(2).gameObject.GetComponent<Text>(); //포밍 캐릭터 소환비용 위치 지정

            FormingCharacterImg.gameObject.GetComponent<RectTransform>().sizeDelta
                = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            FormingCharacterImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(Data.Bottom_FormingPosX, Data.Bottom_FormingPosY, 0);
            FormingCharacterImg.gameObject.GetComponent<Image>().sprite = Data.UnitImage; //포밍 캐릭터 이미지 사이즈, 위치, 종류 조정

            FormingCharacterName.text = Data.UnitName;
            FormingCharacterCost.text = $"{Data.UnitCost.ToString()}원"; //포밍 캐릭터 소환 비용, 이름 설정

            CharacterList.Add(FormingType); //소환 가능 리스트에 추가

            Debug.Log($"{Data.UnitImage.name}: ({Data.UnitImage.texture.width},{Data.UnitImage.texture.height})");

            //if (Data.UnitLevel > 0)
            //{
            //    GameObject FormingType = Instantiate(FormingBox);
            //    FormingType.GetComponent<FormingBoxData>().BoxData = Data;//포밍 박스 데이터 삽입

            //    FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            //    FormingType.transform.parent = Contents.transform;
            //    FormingType.transform.localScale = Vector3.one; //포밍 박스 게임 속 위치 조정

            //    FormingCharacterImg = FormingType.transform.GetChild(0).transform.GetChild(0).gameObject; //포밍 캐릭터 이미지 위치 지정
            //    FormingCharacterName = FormingType.transform.GetChild(1).gameObject.GetComponent<Text>(); //포밍 캐릭터 이름 위치 지정
            //    FormingCharacterCost = FormingType.transform.GetChild(2).gameObject.GetComponent<Text>(); //포밍 캐릭터 소환비용 위치 지정

            //    FormingCharacterImg.gameObject.GetComponent<RectTransform>().sizeDelta
            //        = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            //    FormingCharacterImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(Data.Bottom_FormingPosX, Data.Bottom_FormingPosY, 0);
            //    FormingCharacterImg.gameObject.GetComponent<Image>().sprite = Data.UnitImage; //포밍 캐릭터 이미지 사이즈, 위치, 종류 조정

            //    FormingCharacterName.text = Data.UnitName;
            //    FormingCharacterCost.text = $"{Data.UnitCost.ToString()}원"; //포밍 캐릭터 소환 비용, 이름 설정

            //    CharacterList.Add(FormingType); //소환 가능 리스트에 추가

            //    Debug.Log($"{Data.UnitImage.name}: ({Data.UnitImage.texture.width},{Data.UnitImage.texture.height})");
            //}
        }
    }

    private void ObjectDrag() // Clone생성 변수
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 눌렀을 때
        {
            StartMB_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 스타트 마우스 위치 변수에 게임 화면 속 마우스 y좌표 값 삽입
        }

        if(Input.GetMouseButton(0)) // 마우스 누르고 있을 때
        {
            SettingClone();
        }

        if (Input.GetMouseButtonUp(0)) // 마우스 땟을 때
        {
            ResetCloneData();
        }
    }

    void SettingClone()
    {
        Debug.Log("DownIng");
        CurMB_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 현재 마우스 위치 변수에 게임 화면 속 마우스 y좌표 값 삽입

        if (CurMB_Pos.y > StartMB_Pos.y + 2.0f && SelectFormingUnit != null && SetClone == false) // 클론이 생성되기 위한 조건
        {
            SetClone = true;
            ScrollView.horizontal = false; // 스크롤 중지
            CloneBox = Instantiate(CloneBoxData, CurMB_Pos, Quaternion.identity); // clone생성

            CloneBox.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SelectFormingUnit.UnitImage;  
            CloneBox.transform.GetChild(0).gameObject.transform.position = new Vector3(CurMB_Pos.x + SelectFormingUnit.Clone_FormingPosX, CurMB_Pos.y + SelectFormingUnit.Clone_FormingPosY, 0);
            Debug.Log(SelectFormingUnit.UnitName);
        }

        if (SetClone == true && CloneBox != null)
        {
            CloneBox.transform.position = CurMB_Pos; // clone 마우스 위치로 이동
            CloneBox.transform.GetChild(0).gameObject.transform.position = new Vector3(CurMB_Pos.x + SelectFormingUnit.Clone_FormingPosX, CurMB_Pos.y + SelectFormingUnit.Clone_FormingPosY, 0);
        }
    }

    void ResetCloneData() // 클론 데이터 초기화
    {
        Debug.Log("Up");

        Destroy(CloneBox); // 클론 삭제
        ScrollView.horizontal = true; // 스크롤 재가동
        SetClone = false;
        StartMB_Pos = new Vector2(0, 0); // 스타트 마우스 위치 변수 초기화
        CurMB_Pos = new Vector2(0,0);// 현재 마우스 위치 변수 초기화

        SelectFormingUnit = null; // 선택 된 유닛 초기화
    }

    void AddUnitBtn()
    {
        GameObject Unit = new GameObject();
        Unit.transform.parent = Contents.transform;
    }
}
