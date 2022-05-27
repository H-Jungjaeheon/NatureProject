using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FormingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Contents;
    [SerializeField]
    private GameObject FormingBox;

    [SerializeField]
    private List<GameObject> CharacterList;

    [SerializeField]
    private GameObject CharacTerImgBox;
    [SerializeField]
    private List<GameObject> FormingBoxs;

    public GameUnitData SelectFormingUnit;

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
            //    GameObject FormingType = Instantiate(FormingBoxImage);
            //    FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            //    FormingType.transform.parent = Contents.transform;
            //    FormingType.transform.localScale = Vector3.one;
            //}
        }
    }

    private void ObjectDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Down");
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Up");
            SelectFormingUnit = null;
        }
    }

    void AddUnitBtn()
    {
        GameObject Unit = new GameObject();
        Unit.transform.parent = Contents.transform;
    }
}
