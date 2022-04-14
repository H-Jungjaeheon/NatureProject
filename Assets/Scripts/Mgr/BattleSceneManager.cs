using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : SingletonMono<BattleSceneManager>
{
    public bool IsPass; //소환 버튼 넘김
    public float PlayerHp, MaxPlayerHp, EnemyHp, MaxEnemyHp, Money, MaxMoney, FireCoolTime,
    MaxFireCoolTime, MoneyLevel, MaxMoneyLevel, UpgradeNeedMoney, Timer;
    public int Min = 0, ST = 0;
    public Text MoneyText, MoneyLevelText, UpgradeNeedMoneyText, StageText, TimeText;
    public Image PlayerHpBar, EnemyHpBar, FireCoolTimeImage;
    [SerializeField] private Vector2 Pos, Pos2;
    [SerializeField] private bool IsTouch;
    private void Start()
    {
        Timer = 0;
        Money = 0;
        MaxMoney = 100;
        FireCoolTime = 100;
        MoneyLevel = 1;
        MaxFireCoolTime = 100;
        UpgradeNeedMoney = 40; //돈 차는 속도 or 돈 총량 레벨 비례 올리기
    }
    private void Update()
    {
        DragInput();
        BattleUI();
        BattleAmounts();
    }
    private void FixedUpdate()
    {
        SpawnButtonMove();       
    }
    void BattleAmounts()
    {
        Money += Time.deltaTime * (2 + MoneyLevel); //나중에 10에다가 돈 업그레이드 레벨 or 레벨당 수치만큼 더해주기
        Timer += Time.deltaTime;
        FireCoolTime -= Time.deltaTime * (5); //나중에 5에다가 쿨타임 업그레이드 레벨 or 레벨당 수치만큼 더해주기
        if(Money >= MaxMoney)       
            Money = MaxMoney;
        if(FireCoolTime <= 0)
            FireCoolTime = 0;
    }
    void BattleUI()
    {
        if (Timer > 1)
        {
            Timer = 0;
            ST++;
        }
        if(ST > 59)
        {
            Min++;
            ST = 0;
        }
        MoneyText.text = $"{Money:N0} / {MaxMoney} 원";
        TimeText.text = string.Format($"{Min:D2} : {ST:D2}");
        //StageText.text = ""; 나중에 스테이지 정보마다 바꾸기
        MoneyLevelText.text = $"Level.{MoneyLevel:N0}";
        PlayerHpBar.fillAmount = PlayerHp / MaxPlayerHp;
        EnemyHpBar.fillAmount = EnemyHp / MaxEnemyHp;
        FireCoolTimeImage.fillAmount = FireCoolTime / MaxFireCoolTime;
        if (MoneyLevel < MaxMoneyLevel)
            UpgradeNeedMoneyText.text = $"{UpgradeNeedMoney:N0} 원";
        else
            UpgradeNeedMoneyText.text = "Max";
        if (PlayerHp >= MaxPlayerHp)
            PlayerHp = MaxPlayerHp;
        if (EnemyHp >= MaxEnemyHp)
            EnemyHp = MaxEnemyHp;
        if (FireCoolTime <= 0)
            FireCoolTime = 0;
    }
    void DragInput()
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
    void SpawnButtonMove()
    {
        if (IsTouch == true)
        {
            Pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Pos2.y < Pos.y - 1 && Pos.x + 0.6f >= Pos2.x && Pos.x - 0.6f <= Pos2.x)
            {
                IsTouch = false;
                if (IsPass == false)
                    IsPass = true;
                else
                    IsPass = false;
            }
        }
    }
    public void UpGradeMoney()
    {
        if(MoneyLevel < MaxMoneyLevel && Money >= UpgradeNeedMoney)
        {
            Money -= UpgradeNeedMoney;
            MoneyLevel++;
            UpgradeNeedMoney += 40; //돈 차는 속도 or 돈 총량 레벨 비례 올리기
            MaxMoney += 40; //돈 총량 레벨 비례 올리기
        }
    }
    public void Fire()
    {
        if(FireCoolTime <= 0)
        {
            FireCoolTime = MaxFireCoolTime;
            //대포 발사 능력 작동
        }
    }
    public void GameStopButton()
    {
        Time.timeScale = 0;
        //멈춤 창 띄우기
    }
}
