using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] GameObject[] Units;
    [SerializeField] Button[] SpawnButtons;
    [SerializeField] Image[] SpawnButtonsUnitsImages;
    [SerializeField] Text[] SpawnButtonsUnitsCost;

    void Start()
    {
        StartSettings();
    }

    void Update()
    {
        ButtonSettings();
    }

    private void StartSettings()
    {
        for (int a = 0; a <= 7; a++)
        {
            if(a < 4)
            {
                int temp = a + 4;
                SpawnButtonsUnitsImages[a].sprite = GameManager.In.InGameFormingData[temp].UnitImage;
                SpawnButtonsUnitsCost[a].text = $"{GameManager.In.InGameFormingData[temp].UnitCost} ¿ø";
                SpawnButtons[a].onClick.AddListener(() => Spawn(GameManager.In.InGameFormingData[temp].UnitID, temp));
            }
            else
            {
                int temp = a - 4;
                SpawnButtonsUnitsImages[a].sprite = GameManager.In.InGameFormingData[temp].UnitImage;
                SpawnButtonsUnitsCost[a].text = $"{GameManager.In.InGameFormingData[temp].UnitCost} ¿ø";
                SpawnButtons[a].onClick.AddListener(() => Spawn(GameManager.In.InGameFormingData[temp].UnitID, temp));
            }
        }
    }
    public void Spawn(int UnitID, int UnitData)
    {
        if (BattleSceneManager.In.IsStop == false && GameManager.In.InGameFormingData[UnitData].UnitCost <= BattleSceneManager.In.Money)
        {
            Instantiate(Units[UnitID - 1], new Vector3(-2, 0.2f, 0), Units[UnitID - 1].transform.rotation); //-1.25
            BattleSceneManager.In.Money -= GameManager.In.InGameFormingData[UnitData].UnitCost;
            StartCoroutine(SpawnCastleAnim());
        }
    }
    private void ButtonSettings()
    {
        if (BattleSceneManager.In.IsPass == true)
        {
            for (int a = 0; a < 4; a++)
            {
                Color color = SpawnButtons[a].GetComponent<Image>().color;
                color = new Color(115, 115, 115, 255);
                SpawnButtons[a].GetComponent<Image>().color = color;
                SpawnButtons[a].transform.SetAsLastSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = true;
            }
            for (int a = 4; a < 8; a++)
            {
                SpawnButtons[a].transform.SetAsFirstSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (int a = 0; a < 4; a++)
            {
                SpawnButtons[a].transform.SetAsFirstSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = false;
            }
            for (int a = 4; a < 8; a++)
            {
                Color color = SpawnButtons[a].GetComponent<Image>().color;
                color = new Color(115, 115, 115, 255);
                SpawnButtons[a].GetComponent<Image>().color = color;
                SpawnButtons[a].transform.SetAsLastSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = true;
            }
        }
    }
    IEnumerator SpawnCastleAnim()
    {
        BattleSceneManager.In.Castle.transform.DOScale(0.95f, 0.3f).SetEase(Ease.OutBack);
        BattleSceneManager.In.Castle.transform.DOScaleX(0.85f, 0.3f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.3f);
        BattleSceneManager.In.Castle.transform.DOScaleY(1.05f, 0.1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.1f);
        BattleSceneManager.In.Castle.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        yield return null;
    }
}