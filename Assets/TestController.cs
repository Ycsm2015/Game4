using System.Collections;
using System.Collections.Generic;
using MYUIFW;
using UnityEngine;

public class TestController : MonoBehaviour
{
    private bool isConsoleShow = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.BackQuote))
        {
            if (!isConsoleShow)
            {
                Debug.Log("呼出控制台");
                UIManager.GetInstance().ShowUIForm("ConsoleWindow");
                isConsoleShow = true;
            }

        }
    }
}
