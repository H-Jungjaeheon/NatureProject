using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [Header("���� �⺻ ����")]
    public int MaxEnergy;
    public int Energy;
    public int Food;
    public int Money;

    private void Start()
    {
        BasicSetting();
    }

    void BasicSetting()
    {
        MaxEnergy = 100;
        Energy = MaxEnergy;
    }
}
