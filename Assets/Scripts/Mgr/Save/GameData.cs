using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable] // 직렬화

public class GameData 
{
    [Header("Stage_변수")]
    public int UnlockStage;
    public int SelectStage;
    [Space (10)]

    public List<StageInfo> StageInfos;

    // 각 챕터의 잠금여부
    //public bool isClear2; 
    //public bool isClear3; 
    //public bool isClear4; 
    //public bool isClear5; 
}