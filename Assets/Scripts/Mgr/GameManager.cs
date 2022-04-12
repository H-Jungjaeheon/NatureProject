using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    public bool IsPass; //소환 버튼 넘김
    public float PlayerHp, MaxPlayerHp, EnemyHp, MaxEnemyHp, Money, MaxMoney, FireCoolTime;
    public Text MoneyText, UpgradeMoneyText, UpgradeNeedMoneyText, StageText;
    
}
