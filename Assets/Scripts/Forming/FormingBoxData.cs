using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class FormingBoxData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FormingManager formingManager;
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
    public void OnPointerUp(PointerEventData eventData)
    {

    }

    void InsertForming()
    {
        formingManager.SelectFormingUnit = BoxData;
    }
}
