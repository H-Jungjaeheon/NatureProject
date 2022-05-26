using UnityEngine;

[System.Serializable]
public class GameUnitData
{
    public enum CharacterClass
    {
        Common,
        Rare,
        Unique,
        Legendary,
    }

    [Header("�ʼ� ����")]
    public int UnitID;
    public string UnitName;
    public CharacterClass UnitClass;
    public Sprite UnitImage;
    public int UnitLevel;
    public int UnitCost;
    public int UnitPower;
    public int UnitHP;

    [Header("���׷��̵� ����")]
    public int UpgreadPower;
    public int UpgreadHp;

    [Header("���� ����")]
    public float Up_FormingPosX;
    public float Up_FormingPosY;
    public float Bottom_FormingPosX;
    public float Bottom_FormingPosY;
}
