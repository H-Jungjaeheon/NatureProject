using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    const int MaxLevel = 20;

    [Header("게임 기본 변수")]
    public int MaxEnergy;
    public int Energy;
    public int Food;
    public int Money;

    [Header("배틀씬 기본 변수")]
    [SerializeField]
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

    private void Start()
    {
        BasicSetting();
    }

    void BasicSetting()
    {
        MaxEnergy = 100;
        Energy = MaxEnergy;
    }
}
