using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    public bool IsPass; //¼ÒÈ¯ ¹öÆ° ³Ñ±è
    public float PlayerHp, MaxPlayerHp, EnemyHp, MaxEnemyHp, Money, MaxMoney, FireCoolTime;
    public Text MoneyText, UpgradeMoneyText, UpgradeNeedMoneyText, StageText;
    [SerializeField] private Vector2 Pos, Pos2;
    [SerializeField] private bool IsTouch;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            IsTouch = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsTouch = false;
        }       
    }
    private void FixedUpdate()
    {
        if (IsTouch == true)
        {
            Pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Pos2.y < Pos.y && Pos2.x == Pos.x)
            {
                Debug.Log("¿Ö ¾ÈµÊ");
                IsTouch = false;
                if (IsPass == false)
                    IsPass = true;
                else
                    IsPass = false;
            }
        }
    }
}
