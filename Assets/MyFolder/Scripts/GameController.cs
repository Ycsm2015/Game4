using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MYUIFW;
public class GameController : MonoBehaviour
{
    private bool isConsoleWindowShow;
    // Use this for initialization
    void Start()
    {
        UIManager.GetInstance().ShowUIForm("StartGame");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!isConsoleWindowShow)

                ShowConsole();
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowConsole()
    {
        UIManager.GetInstance().ShowUIForm("ConsoleWindow");
    }
}
