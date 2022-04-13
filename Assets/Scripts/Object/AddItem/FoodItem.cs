using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MainItem
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void BasicSetting()
    {
        AddCount = Random.Range(2, 5);
        base.BasicSetting();
    }

    protected override void DestroyObject()
    {
        GameManager.In.Food += AddCount;

        base.DestroyObject();
    }
}
