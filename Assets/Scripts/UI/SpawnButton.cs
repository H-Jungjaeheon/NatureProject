using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] GameObject[] SpawnButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleSceneManager.In.IsPass == true)
        {
            for (int a = 0; a < 5; a++)
            {
                SpawnButtons[a].transform.SetAsLastSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = true;
            }
            for (int a = 5; a < 10; a++)
            {
                SpawnButtons[a].transform.SetAsFirstSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (int a = 0; a < 5; a++)
            {
                SpawnButtons[a].transform.SetAsFirstSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = false;
            }
            for (int a = 5; a < 10; a++)
            {
                SpawnButtons[a].transform.SetAsLastSibling();
                SpawnButtons[a].GetComponent<Button>().interactable = true;
            }
        }
    }
}
