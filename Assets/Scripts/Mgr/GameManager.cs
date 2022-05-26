using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    public int MaxLevel = 20;

    [Header("게임 기본 변수")]
    public int MaxEnergy;
    public int Energy;
    public int Food;
    public int Money;

    [Header("배틀씬 기본 변수")]
    [SerializeField]
    #region 비둘기 차량 관련
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

    #region 전투 편성
    [Space(5)]
    public List<GameUnitData> FormingData;
    #endregion

    [Header("스테이지 선택 기본 변수")]
    public int UnlockStage = 1;
    public int SelectStage = 1;
    public Dictionary<string, StageInfo> DicStageInfo;

    [Header("UnitData 변수")]
    public List<GameUnitData> GameUnitData;

    [Header("게임 저장 변수")]
    public string GameDataFileName = "StageData.json";

    // "원하는 이름(영문).json"
    [SerializeField]
    private GameData _gameData;
    public GameData gameData
    {
        get
        {
            // 게임이 시작되면 자동으로 실행되도록
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

    // 저장된 게임 불러오기
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        Debug.Log(filePath);

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            print("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }

        // 저장된 게임이 없다면
        //else
        //{
        //    print("새로운 파일 생성");
        //    _gameData = new GameData();
        //}
    }

    // 게임 저장하기
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);

        // 올바르게 저장됐는지 확인 (자유롭게 변형)
        print("저장완료");
    }

    // 게임을 종료하면 자동저장되도록
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
