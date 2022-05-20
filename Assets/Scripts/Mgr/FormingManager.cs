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
        foreach (GameUnitData Data in GameManager.In.GameUnitData)
        {
            GameObject FormingType = Instantiate(FormingBoxImage);
            FormingType.name = $"{Data.UnitName.ToString()}_FormingBox";
            FormingType.transform.parent = Contents.transform;
            FormingType.transform.localScale = Vector3.one;
            FormingType.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(Data.UnitImage.textureRect.width, Data.UnitImage.textureRect.height);
            FormingType.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Data.UnitImage;
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
