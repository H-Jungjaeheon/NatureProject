using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyItem : MainItem
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void BasicSetting()
    {
        AddCount = Random.Range(100, 251);
        base.BasicSetting();
    }

    protected override void DestroyObject()
    {
        GameManager.In.Money += AddCount;

        base.DestroyObject();
    }
}
