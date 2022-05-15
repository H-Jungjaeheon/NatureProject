using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleSceneManager : SingletonMono<BattleSceneManager>
{
    #region ������ ���� ���� ����
    [Header("������ ���� ������")]
    public bool IsPass;
    public bool IsStop, IsOut, IsFire;
    public float PlayerHp, MaxPlayerHp, EnemyHp, MaxEnemyHp, Money, MaxMoney, FireCoolTime,
    MaxFireCoolTime, MoneyLevel, MaxMoneyLevel, UpgradeNeedMoney, Timer, FireDamage;
    private float WeelRt, DoorRt;
    [SerializeField] private Vector2 Pos, Pos2;
    [SerializeField] private bool IsTouch, IsStart, IsStart2;
    [SerializeField] private float MaxFireHitCount;
    [Header("������ �ð� �� ����")]
    public int Min = 0;
    public int ST = 0;
    [Header("�� ��")]
    [SerializeField] private bool IsDraging; //�巡����
    [SerializeField] private Vector2 CamCenter, CamMoveSize; //ī�޶� �̵� ���� �߾�, ī�޶� �̵� ���� ����
    [SerializeField] private float Height, Width, CamMoveSpeed; //ī�޶� �̵� �ӵ�
    #endregion
    #region ������ UI ����
    [Header("������ �ؽ�Ʈ UI")]
    public Text MoneyText;
    public Text MoneyLevelText, UpgradeNeedMoneyText, StageText, TimeText;
    [Header("������ �̹��� UI")]
    public Image PlayerHpBar;
    public Image EnemyHpBar, FireCoolTimeImage;
    [SerializeField] private RawImage StopBG, BlackFade;
    #endregion
    #region ������ ������Ʈ ����
    [Header("������ ���� ������Ʈ")]
    [SerializeField] private Camera MainCam;
    [SerializeField] private GameObject SolaPanel, FireEffect;
    [SerializeField] private GameObject[] Weel, Enemys;
    [SerializeField] private Sprite[] SolaSprite;
    [SerializeField] private SpriteRenderer SSR;
    public GameObject ExitObj, SoundObj, Castle, CastleBody, CastleDoor, NullFireButton, PauseObj;
    #endregion

    private void Start()
    {
        StartSetting();
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
        Starts();
        EnemyFind();
        CamMoveLimit();
        CamMove();
    }
    #region ���� ���� ���� �� �ִϸ��̼�
    private void StartSetting()
    {
        IsOut = false;
        IsStart = true;
        IsStop = true;
        Timer = 0;
        Money = 0;
        MaxMoney = 100;
        FireCoolTime = 100;
        MoneyLevel = 1;
        MaxFireCoolTime = 100;
        UpgradeNeedMoney = 40; //�� ���� �ӵ� or �� �ѷ� ���� ��� �ø���
        SSR = SolaPanel.GetComponent<SpriteRenderer>();
        Color BC = BlackFade.GetComponent<RawImage>().color;
        BC.a = 0;
        BlackFade.GetComponent<RawImage>().color = BC;
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;
        StartCoroutine(StartCastle());      
    }
    private void CamMoveLimit()
    {
        MainCam.transform.position = MainCam.transform.position;
        float LimitX = CamMoveSize.x * 0.5f - Width;
        float LimitY = CamMoveSize.y * 0.5f - Height;
        float clampX = Mathf.Clamp(MainCam.transform.position.x, -LimitX + CamCenter.x, LimitX + CamCenter.x);
        float clampY = Mathf.Clamp(MainCam.transform.position.y, -LimitY + CamCenter.y, LimitY + CamCenter.y);
        MainCam.transform.position = new Vector3(clampX, MainCam.transform.position.y, MainCam.transform.position.z);
    }
    #region ī�޶� �̵� ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CamCenter, CamMoveSize);
    }
    #endregion
    private IEnumerator StartCastle()
    {
        StartCoroutine(StartCastle2());
        yield return new WaitForSeconds(4);
        IsStart = false;
        IsStart2 = true;
        StartCoroutine(StartCastle3());
        yield return new WaitForSeconds(3);
        IsStart2 = false;
        IsStop = false;
        yield return null;
    }
    private IEnumerator StartCastle2()
    {
        while (IsStart == true)
        {
            if (CastleBody.transform.position.y == 0.27f)
            {
                CastleBody.transform.position = new Vector2(CastleBody.transform.position.x, 0.23f);
                yield return new WaitForSeconds(0.09f);
            }
            else if (CastleBody.transform.position.y == 0.23f)
            {
                CastleBody.transform.position = new Vector2(CastleBody.transform.position.x, 0.27f);
                yield return new WaitForSeconds(0.09f);
            }
            else
            {
                CastleBody.transform.position = new Vector2(CastleBody.transform.position.x, 0.27f);
                yield return new WaitForSeconds(0.09f);
            }
        }
        CastleBody.transform.position = new Vector2(CastleBody.transform.position.x, 0.3f);
        yield return null;
    }
    private IEnumerator StartCastle3()
    {
        while (IsStart2 == true)
        {
            if(DoorRt < 70)
                DoorRt += 1;           
            CastleDoor.transform.rotation = Quaternion.Euler(0, 0, DoorRt);
            yield return new WaitForSeconds(0.03f);
        }
        yield return null;
    }
    private void Starts()
    {
        float CastleX = Castle.transform.position.x;
        if (IsStart == true && IsOut == false)
        {
            if (CastleX < -5)
                WeelRt += Time.deltaTime * 250;
            Castle.transform.position = Vector3.MoveTowards(Castle.transform.position, new Vector3(-5, 0.3f, 0), Time.deltaTime * 3f);
            for (int a = 0; a < 2; a++)
                Weel[a].transform.rotation = Quaternion.Euler(0, 0, -WeelRt);
        }
        if (IsStart2 == true && IsOut == false)
        {
            CastleDoor.transform.position = Vector3.MoveTowards(CastleDoor.transform.position, new Vector3(-1.4f, 2f, 0), Time.deltaTime * 0.5f);
        }
    }
    #endregion
    private void BattleAmounts()
    {
        if (IsStop == false && IsOut == false)
        {
            Money += Time.deltaTime * (2 + MoneyLevel); //���߿� 10���ٰ� �� ���׷��̵� ���� or ������ ��ġ��ŭ �����ֱ�
            Timer += Time.deltaTime;
            FireCoolTime -= Time.deltaTime * (5); //���߿� 5���ٰ� ��Ÿ�� ���׷��̵� ���� or ������ ��ġ��ŭ �����ֱ�
            if (Money >= MaxMoney)
                Money = MaxMoney;
            if (FireCoolTime <= 0)
                FireCoolTime = 0;
        }
    }
    private void EnemyFind()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }
    private void BattleUI()
    {
        if (IsStop == false && IsOut == false)
        {
            if (Timer > 1)
            {
                Timer = 0;
                ST++;
            }
            if (ST > 59)
            {
                Min++;
                ST = 0;
            }
            MoneyText.text = $"{Money:N0} / {MaxMoney} ��";
            TimeText.text = string.Format($"{Min:D2} : {ST:D2}");
            //StageText.text = ""; ���߿� �������� �������� �ٲٱ�
            MoneyLevelText.text = $"Level.{MoneyLevel:N0}";
            PlayerHpBar.fillAmount = PlayerHp / MaxPlayerHp;
            EnemyHpBar.fillAmount = EnemyHp / MaxEnemyHp;
            FireCoolTimeImage.fillAmount = FireCoolTime / MaxFireCoolTime;
            if (MoneyLevel < MaxMoneyLevel)
                UpgradeNeedMoneyText.text = $"{UpgradeNeedMoney:N0} ��";
            else
                UpgradeNeedMoneyText.text = "Max";
            if (PlayerHp >= MaxPlayerHp)
                PlayerHp = MaxPlayerHp;
            if (EnemyHp >= MaxEnemyHp)
                EnemyHp = MaxEnemyHp;
            if (FireCoolTime <= 0)
                FireCoolTime = 0;
            Color a = StopBG.GetComponent<RawImage>().color;
            a.a = 0;
            StopBG.GetComponent<RawImage>().color = a;
        }

        if(FireCoolTime <= 0 && IsOut == false)
        {
            NullFireButton.SetActive(false);
        }
        else if(FireCoolTime > 0 && IsOut == false)
            NullFireButton.SetActive(true);
    }
    private void DragInput()
    {
        if (IsStop == false && IsOut == false)
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
    }
    private void SpawnButtonMove()
    {
        if (IsTouch == true && IsStop == false && IsOut == false)
        {
            Pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CamMoveSpeed = 0;
            if (Pos2.y < Pos.y - 1 && Pos.x + 0.6f >= Pos2.x && Pos.x - 0.6f <= Pos2.x)
            {
                IsTouch = false;
                if (IsPass == false)
                    IsPass = true;
                else
                    IsPass = false;
            }
            else if (Pos.x + 0.6f < Pos2.x || Pos.x - 0.6f > Pos2.x && IsDraging == false)
            {
                StartCoroutine(Dragings());
                CamMoveSpeed = (Pos2.x - Pos.x);
            }
            else if(Pos.x + 0.6f >= Pos2.x || Pos.x - 0.6f <= Pos2.x)
            {
                MainCam.transform.position = new Vector3(MainCam.transform.position.x + (Pos.x - Pos2.x), MainCam.transform.position.y, MainCam.transform.position.z);
            }
        }
    }
    private void CamMove() => MainCam.transform.position = new Vector3(MainCam.transform.position.x - (CamMoveSpeed * Time.deltaTime), MainCam.transform.position.y, MainCam.transform.position.z);
    private IEnumerator Dragings()
    {
        yield return new WaitForSeconds(0.07f);
        IsTouch = false;
        IsDraging = true;
        yield return new WaitForSeconds(0.1f);
        IsDraging = false;
        Pos2 = new Vector2(0, 0);
        yield return null;
    }
    private void UpGradeMoney()
    {
        if (MoneyLevel < MaxMoneyLevel && Money >= UpgradeNeedMoney && IsStop == false && IsOut == false)
        {
            Money -= UpgradeNeedMoney;
            MoneyLevel++;
            UpgradeNeedMoney += 40; //�� ���� �ӵ� or �� �ѷ� ���� ��� �ø���
            MaxMoney += 40; //�� �ѷ� ���� ��� �ø���
        }
    }
    private void Fire()
    {
        if(FireCoolTime <= 0 && IsStop == false && IsOut == false && IsFire == false)
        {
            IsFire = true;
            FireCoolTime = MaxFireCoolTime;
            SSR.sprite = SolaSprite[1];
            StartCoroutine(FireTruckAnim());
            StartCoroutine(Lazer());
            //���� �߻� �ɷ� �۵�
        }
    }
    private IEnumerator Lazer()
    {
        for (int LazerHitCount = 0; LazerHitCount < MaxFireHitCount; LazerHitCount++)
        {
            for (int a = 0; a < Enemys.Length; a++)
            {
                if (Enemys[a].GetComponent<BasicEnemy>().IsKnockBack == false)
                {
                    Enemys[a].GetComponent<BasicEnemy>().Hp -= FireDamage;
                    Enemys[a].GetComponent<BasicEnemy>().ReceivDamage += FireDamage;
                }
            }
            yield return new WaitForSeconds(1.5f / MaxFireHitCount);
        }
        IsFire = false;
        SSR.sprite = SolaSprite[0];
        yield return null;
    }
    private IEnumerator FireTruckAnim()
    {
        Instantiate(FireEffect, new Vector2(SolaPanel.transform.position.x - 0.6f, SolaPanel.transform.position.y + 1.9f), SolaPanel.transform.rotation);
        Instantiate(FireEffect, new Vector2(SolaPanel.transform.position.x, SolaPanel.transform.position.y + 1.5f), SolaPanel.transform.rotation);
        Instantiate(FireEffect, new Vector2(SolaPanel.transform.position.x + 1f, SolaPanel.transform.position.y + 1.7f), SolaPanel.transform.rotation);
        while (IsFire == true)
        {
            if (CastleBody.transform.position.x == -5f)
            {
                CastleBody.transform.position = new Vector2(-4.98f, CastleBody.transform.position.y);
                yield return new WaitForSeconds(0.05f);
            }
            else if (CastleBody.transform.position.x == -4.98f)
            {
                CastleBody.transform.position = new Vector2(-5.02f, CastleBody.transform.position.y);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                CastleBody.transform.position = new Vector2(-4.98f, CastleBody.transform.position.y);
                yield return new WaitForSeconds(0.05f);
            }
        }
        CastleBody.transform.position = new Vector2(-5f, CastleBody.transform.position.y);
        yield return null;
    }
    #region ���� ���� ����
    public void GamePauseButton()
    {
        if (IsStop == false && IsOut == false)
        {
            IsStop = true;
            StartCoroutine(GamePauseBG());
        }
    }
    IEnumerator GamePauseBG()
    {
        while (true)
        {
            Color a = StopBG.GetComponent<RawImage>().color;
            a.a += Time.deltaTime;
            StopBG.GetComponent<RawImage>().color = a;
            if(a.a >= 0.6f)
            {
                break;
            }
            yield return new WaitForSeconds(0.00001f);
        }
        PauseObj.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        yield return null;
    }
    public void ExitPauseButton()
    {
        if(IsStop== true && IsOut == false)
        {
            IsStop = false;
            StartCoroutine(ExitPauseBG());
        }
    }
    IEnumerator ExitPauseBG()
    {
        PauseObj.transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
        while (true)
        {
            Color a = StopBG.GetComponent<RawImage>().color;
            a.a -= Time.deltaTime;
            StopBG.GetComponent<RawImage>().color = a;
            if (a.a <= 0f)
            {
                break;
            }
            yield return new WaitForSeconds(0.00001f);
        }
        yield return null;
    }
    public void SoundOnButton()
    {
        if (IsOut == false)
            SoundObj.SetActive(true);
    }
    public void SoundExitButton()
    {
        if (IsOut == false)
            SoundObj.SetActive(false);
    }
    public void GameExitButton()
    {
        if(IsOut == false)
           ExitObj.SetActive(true);
    }
    public void CloseGameExitButton()
    {
        if (IsOut == false)
            ExitObj.SetActive(false);
    }
    public void CloseGameButton()
    {
        if (IsOut == false)
            StartCoroutine(CloseGame());
    }
    IEnumerator CloseGame()
    {
        IsOut = true;
        while (true)
        {
            Color a = BlackFade.GetComponent<RawImage>().color;
            a.a += Time.deltaTime;
            BlackFade.GetComponent<RawImage>().color = a;
            if (a.a <= 0f)
            {
                break;
            }
            yield return new WaitForSeconds(0.001f);
        }
        //�������� �� �̵�
        yield return null;
    }
    #endregion
}