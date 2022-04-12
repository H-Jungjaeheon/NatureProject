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
        AddCount = Random.Range(100, 201);
        base.BasicSetting();
    }
}
