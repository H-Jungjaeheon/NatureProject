using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FormingManager : MonoBehaviour
{
    [Header("�� �ڽ� �߰�")]
    [SerializeField]
    private ScrollRect ScrollView; // ��ũ�� �� ��ġ
    [SerializeField]
    private GameObject Contents; // ��ũ�� �� �� ������ ��ġ
    [SerializeField]
    private GameObject FormingBox; // �߰� �� ���� ������Ʈ

    [SerializeField]
    private List<GameObject> CharacterList; // �߰��� ���� �ڽ� ����Ʈ


    [SerializeField]
    private GameObject CharacterImgBox; // ���� ���� ���� �̹���
    [SerializeField]
    private List<GameObject> FormingBoxs; // �� �ڽ� ��ġ

    public GameUnitData SelectFormingUnit; // ���� ������ ���� ������

    [Header("Clone�ڽ� ����")]
    [SerializeField]
    private GameObject CloneBoxData; //���� �ϰ� �巡�� ���� �� ���� CloneBoxData
    private GameObject CloneBox; //���� �ϰ� �巡�� ���� �� ���� CloneBox

    [SerializeField]
    private Vector2 StartMB_Pos; //���콺 ���� ��ġ
    [SerializeField]
    private Vector2 CurMB_Pos; // ���콺 ���� ��ġ
    [SerializeField]
    private bool SetClone = false; // Ŭ���� ���� ����� Ȯ��

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
            //    GameObject FormingType = Instantiate(FormingBox);
            //    FormingType.GetComponent<FormingBoxData>().BoxData = Data;//���� �ڽ� ������ ����

            //    FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            //    FormingType.transform.parent = Contents.transform;
            //    FormingType.transform.localScale = Vector3.one; //���� �ڽ� ���� �� ��ġ ����

            //    FormingCharacterImg = FormingType.transform.GetChild(0).transform.GetChild(0).gameObject; //���� ĳ���� �̹��� ��ġ ����
            //    FormingCharacterName = FormingType.transform.GetChild(1).gameObject.GetComponent<Text>(); //���� ĳ���� �̸� ��ġ ����
            //    FormingCharacterCost = FormingType.transform.GetChild(2).gameObject.GetComponent<Text>(); //���� ĳ���� ��ȯ��� ��ġ ����

            //    FormingCharacterImg.gameObject.GetComponent<RectTransform>().sizeDelta
            //        = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            //    FormingCharacterImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(Data.Bottom_FormingPosX, Data.Bottom_FormingPosY, 0);
            //    FormingCharacterImg.gameObject.GetComponent<Image>().sprite = Data.UnitImage; //���� ĳ���� �̹��� ������, ��ġ, ���� ����

            //    FormingCharacterName.text = Data.UnitName;
            //    FormingCharacterCost.text = $"{Data.UnitCost.ToString()}��"; //���� ĳ���� ��ȯ ���, �̸� ����

            //    CharacterList.Add(FormingType); //��ȯ ���� ����Ʈ�� �߰�

            //    Debug.Log($"{Data.UnitImage.name}: ({Data.UnitImage.texture.width},{Data.UnitImage.texture.height})");
            //}
        }
    }

    private void ObjectDrag() // Clone���� ����
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ������ ��
        {
            StartMB_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ��ŸƮ ���콺 ��ġ ������ ���� ȭ�� �� ���콺 y��ǥ �� ����
        }

        if(Input.GetMouseButton(0)) // ���콺 ������ ���� ��
        {
            SettingClone();
        }

        if (Input.GetMouseButtonUp(0)) // ���콺 ���� ��
        {
            ResetCloneData();
        }
    }

    void SettingClone()
    {
        Debug.Log("DownIng");
        CurMB_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���� ���콺 ��ġ ������ ���� ȭ�� �� ���콺 y��ǥ �� ����

        if (CurMB_Pos.y > StartMB_Pos.y + 2.0f && SelectFormingUnit != null && SetClone == false) // Ŭ���� �����Ǳ� ���� ����
        {
            SetClone = true;
            ScrollView.horizontal = false; // ��ũ�� ����
            CloneBox = Instantiate(CloneBoxData, CurMB_Pos, Quaternion.identity); // clone����

            CloneBox.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = SelectFormingUnit.UnitImage;  
            CloneBox.transform.GetChild(0).gameObject.transform.position = new Vector3(CurMB_Pos.x + SelectFormingUnit.Clone_FormingPosX, CurMB_Pos.y + SelectFormingUnit.Clone_FormingPosY, 0);
            Debug.Log(SelectFormingUnit.UnitName);
        }

        if (SetClone == true && CloneBox != null)
        {
            CloneBox.transform.position = CurMB_Pos; // clone ���콺 ��ġ�� �̵�
            CloneBox.transform.GetChild(0).gameObject.transform.position = new Vector3(CurMB_Pos.x + SelectFormingUnit.Clone_FormingPosX, CurMB_Pos.y + SelectFormingUnit.Clone_FormingPosY, 0);
        }
    }

    void ResetCloneData() // Ŭ�� ������ �ʱ�ȭ
    {
        Debug.Log("Up");

        Destroy(CloneBox); // Ŭ�� ����
        ScrollView.horizontal = true; // ��ũ�� �簡��
        SetClone = false;
        StartMB_Pos = new Vector2(0, 0); // ��ŸƮ ���콺 ��ġ ���� �ʱ�ȭ
        CurMB_Pos = new Vector2(0,0);// ���� ���콺 ��ġ ���� �ʱ�ȭ

        SelectFormingUnit = null; // ���� �� ���� �ʱ�ȭ
    }

    void AddUnitBtn()
    {
        GameObject Unit = new GameObject();
        Unit.transform.parent = Contents.transform;
    }
}
