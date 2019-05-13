using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadFirstLevel(PlayerData data)
    {
        Player.MyInstance.transform.position = data.position;
        Player.MyInstance.MyHealth.MyMaxValue = data.maxHealth;
        Player.MyInstance.MyHealth.MyCurrentValue = data.health;
        Player.MyInstance.MyLevel = data.level;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DoExitGame()
    {
        Application.Quit();
    }
}
