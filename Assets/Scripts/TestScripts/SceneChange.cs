using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : SingletonMono<SceneChange>
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ThisSceneChange();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SoundManager.In.SettingBGM("BasicBGM");
        }

        else if (Input.GetKeyDown(KeyCode.K))
        {
            SoundManager.In.SettingBGM("ChangeBGM");
        }
    }

    void ThisSceneChange()
    {
        if(SceneManager.GetActiveScene().name.Equals("Test1"))
        {
            SceneManager.LoadScene("Test2");
        }

        else if(SceneManager.GetActiveScene().name.Equals("Test2"))
        {
            SceneManager.LoadScene("Test1");
        }
    }
}
