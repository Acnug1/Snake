using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayHandler()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // загружаем сцену с игрой
    }

    public void OnExitHandler()
    {
        Application.Quit();
    }
}
