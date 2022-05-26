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
    private GameObject FormingBoxImage;
    [SerializeField]
    private List<GameObject> FormingBoxs; 

    // Start is called before the first frame update
    void Start()
    {
        SceneSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SceneSetting()
    {
        GameObject FormingCharacterImg = null;
        Text FormingCharacterName = null;
        Text FormingCharacterCost = null;

        foreach (GameUnitData Data in GameManager.In.GameUnitData)
        {
            GameObject FormingType = Instantiate(FormingBoxImage);
            FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            FormingType.transform.parent = Contents.transform;
            FormingType.transform.localScale = Vector3.one;

            FormingCharacterImg = FormingType.transform.GetChild(0).transform.GetChild(0).gameObject;
            FormingCharacterName = FormingType.transform.GetChild(1).gameObject.GetComponent<Text>();
            FormingCharacterCost = FormingType.transform.GetChild(2).gameObject.GetComponent<Text>();

            FormingCharacterImg.gameObject.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            FormingCharacterImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(Data.Bottom_FormingPosX, Data.Bottom_FormingPosY, 0);
            FormingCharacterImg.gameObject.GetComponent<Image>().sprite = Data.UnitImage;

            FormingCharacterName.text = Data.UnitName;
            FormingCharacterCost.text = $"{Data.UnitCost.ToString()}¿ø";

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

    void AddUnitBtn()
    {
        GameObject Unit = new GameObject();
        Unit.transform.parent = Contents.transform;
    }
}
