using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    public bool IsPass; //��ȯ ��ư �ѱ�
    public float PlayerHp, MaxPlayerHp, EnemyHp, MaxEnemyHp, Money, MaxMoney, FireCoolTime;
    public Text MoneyText, UpgradeMoneyText, UpgradeNeedMoneyText, StageText;
    
}
