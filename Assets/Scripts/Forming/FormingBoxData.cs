using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class FormingBoxData : MonoBehaviour, IPointerDownHandler
{
    public FormingManager formingManager;
    public int BoxID;
    public GameUnitData BoxData;


    private void Awake()
    {
        formingManager = GameObject.Find("FomingMgr").GetComponent<FormingManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InsertForming();
        Debug.Log(BoxData.UnitName);
    }

    void InsertForming()
    {
        formingManager.SelectFormingUnit = BoxData;
        formingManager.SelectBoxID = BoxID;
    }
}
