using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerUI : MonoBehaviour
{

    public GameObject settingsWindow;

    //загрузка сцены по его ID
    public void LoadScenes(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void OpenSettingsWindow()
    {
        settingsWindow.SetActive(!settingsWindow.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
