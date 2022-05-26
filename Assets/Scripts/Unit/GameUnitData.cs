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

    [Header("필수 정보")]
    public int UnitID;
    public string UnitName;
    public CharacterClass UnitClass;
    public Sprite UnitImage;
    public int UnitLevel;
    public int UnitCost;
    public int UnitPower;
    public int UnitHP;

    [Header("선택 정보")]
    public float FormingPosX;
    public float FormingPosY;
}
