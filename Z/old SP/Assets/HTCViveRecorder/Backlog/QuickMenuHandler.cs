using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DebugStuff;

public class QuickMenuHandler: MonoBehaviour
{
    int count = 0;
    private bool isOpen = false;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        isOpen = false;
        Debug.Log("Function called");
    }


    void OnGUI()
    {
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(Screen.width - 110, 20, 80, 20), "Menu"))
        {
            if (isOpen == false)
            {
                isOpen = true;
            }
            else
            {
                isOpen = false;
            }
               
            
        }

        if (isOpen)
        {
            GUI.Box(new Rect(Screen.width - 120, 10, 100, 130), "");

            if (GUI.Button(new Rect(Screen.width - 110, 50, 80, 20), "Backlog"))
            {
                ConsoleToGUI.instance.ChangeStatus();
                
            }

            // Make the second button.
            if (GUI.Button(new Rect(Screen.width - 110, 90, 80, 20), "Restart"))
            {
                SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
                Destroy(this.gameObject);
            }

            if (GUI.Button(new Rect(Screen.width - 110, 110, 80, 20), "QUIT"))
            {
                Application.Quit();
            }
        }
        else
        {
            GUI.Box(new Rect(Screen.width - 120, 10, 100, 40), "");
        }

        
    }

}
