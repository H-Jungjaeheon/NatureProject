using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [Header("���� �⺻ ����")]
    public int Energy;
    public int Food;
    public int Money;
}
