using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable] // ����ȭ

public class GameData 
{
    [Header("Stage_����")]
    public int UnlockStage;
    public int SelectStage;
    [Space (10)]

    public List<StageInfo> StageInfos;

    // �� é���� ��ݿ���
    //public bool isClear2; 
    //public bool isClear3; 
    //public bool isClear4; 
    //public bool isClear5; 
}