using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [Header("게임 기본 변수")]
    public int Energy;
    public int Food;
    public int Money;
}
