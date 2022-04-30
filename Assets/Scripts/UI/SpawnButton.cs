using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnButton : MonoBehaviour
{
    //List<GameObject> list = new List<GameObject>();
    [SerializeField] GameObject[] SpawnButtons, Units;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleSceneManager.In.IsPass == true)
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
    public void Spawn()
    {
        if(BattleSceneManager.In.IsStop == false)
        {
            Instantiate(Units[0], new Vector3(-1, -1.25f, 0), Units[0].transform.rotation);
            StartCoroutine(SpawnCastleAnim());
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
