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

    void SceneSetting() //���� �� ���� �Լ�
    {
        LoadCharacter();
    }

    void LoadCharacter() // ���� �ڽ� �߰� �Լ�
    {
        GameObject FormingCharacterImg = null;
        Text FormingCharacterName = null;
        Text FormingCharacterCost = null;

        foreach (GameUnitData Data in GameManager.In.GameUnitData) //���� �Ŵ��� �� ���� ������ ��ŭ �ݺ�
        {
            GameObject FormingType = Instantiate(FormingBox);
            FormingType.GetComponent<FormingBoxData>().BoxData = Data;//���� �ڽ� ������ ����

            FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            FormingType.transform.parent = Contents.transform;
            FormingType.transform.localScale = Vector3.one; //���� �ڽ� ���� �� ��ġ ����

            FormingCharacterImg = FormingType.transform.GetChild(0).transform.GetChild(0).gameObject; //���� ĳ���� �̹��� ��ġ ����
            FormingCharacterName = FormingType.transform.GetChild(1).gameObject.GetComponent<Text>(); //���� ĳ���� �̸� ��ġ ����
            FormingCharacterCost = FormingType.transform.GetChild(2).gameObject.GetComponent<Text>(); //���� ĳ���� ��ȯ��� ��ġ ����

            FormingCharacterImg.gameObject.GetComponent<RectTransform>().sizeDelta
                = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            FormingCharacterImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(Data.Bottom_FormingPosX, Data.Bottom_FormingPosY, 0);
            FormingCharacterImg.gameObject.GetComponent<Image>().sprite = Data.UnitImage; //���� ĳ���� �̹��� ������, ��ġ, ���� ����

            FormingCharacterName.text = Data.UnitName; 
            FormingCharacterCost.text = $"{Data.UnitCost.ToString()}��"; //���� ĳ���� ��ȯ ���, �̸� ����

            CharacterList.Add(FormingType); //��ȯ ���� ����Ʈ�� �߰�

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
