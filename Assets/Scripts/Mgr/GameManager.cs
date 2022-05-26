using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    public int MaxLevel = 20;

    [Header("���� �⺻ ����")]
    public int MaxEnergy;
    public int Energy;
    public int Food;
    public int Money;

    [Header("��Ʋ�� �⺻ ����")]
    [SerializeField]
    #region ��ѱ� ���� ����
    private int bodylevel = 1;

    public int BodyLevel
    { 
        get 
        { 
            if(bodylevel >= MaxLevel)
            {
                bodylevel = MaxLevel;
            }

            return bodylevel; 
        } 

        set 
        { 
            bodylevel = value; 
        } 
    }

    [SerializeField]
    private int laserchargelevel = 1;

    public int LaserChargeLevel
    {
        get
        {
            if (laserchargelevel >= MaxLevel)
            {
                laserchargelevel = MaxLevel;
            }

            return laserchargelevel;
        }

        set
        {
            laserchargelevel = value;
        }
    }

    [SerializeField]
    private int laserpowerlevel = 1;

    public int LaserPowerLevel
    {
        get
        {
            if (laserpowerlevel >= MaxLevel)
            {
                laserpowerlevel = MaxLevel;
            }

            return laserpowerlevel;
        }

        set
        {
            laserpowerlevel = value;
        }
    }
    #endregion

    #region ���� ��
    [Space(5)]
    public List<GameUnitData> FormingData;
    #endregion

    [Header("�������� ���� �⺻ ����")]
    public int UnlockStage = 1;
    public int SelectStage = 1;
    public Dictionary<string, StageInfo> DicStageInfo;

    [Header("UnitData ����")]
    public List<GameUnitData> GameUnitData;

    [Header("���� ���� ����")]
    public string GameDataFileName = "StageData.json";

    // "���ϴ� �̸�(����).json"
    [SerializeField]
    private GameData _gameData;
    public GameData gameData
    {
        get
        {
            // ������ ���۵Ǹ� �ڵ����� ����ǵ���
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    private void Start()
    {
        LoadGameData();
        SaveGameData();
        BasicSetting();
    }

    void BasicSetting()
    {
        MaxEnergy = 100;
        Energy = MaxEnergy;
        DicStageInfo = new Dictionary<string, StageInfo>();

        foreach (StageInfo Datas in _gameData.StageInfos)
        {
            //Debug.Log($"{Datas.Chapter}_{Datas.Stage}");
            string Name = $"{Datas.Chapter}_{Datas.Stage}".ToString();

            DicStageInfo.Add(Name, Datas);
        }
    }

    // ����� ���� �ҷ�����
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        Debug.Log(filePath);

        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            print("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }

        // ����� ������ ���ٸ�
        //else
        //{
        //    print("���ο� ���� ����");
        //    _gameData = new GameData();
        //}
    }

    // ���� �����ϱ�
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // �̹� ����� ������ �ִٸ� �����
        File.WriteAllText(filePath, ToJsonData);

        // �ùٸ��� ����ƴ��� Ȯ�� (�����Ӱ� ����)
        print("����Ϸ�");
    }

    // ������ �����ϸ� �ڵ�����ǵ���
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
