using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

namespace DebugStuff
{
    public class ConsoleToGUI : MonoBehaviour
    {
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;
        public bool isActiveGUI = false;
        public static ConsoleToGUI instance;
        public Vector2 scrollPosition = Vector2.zero;



        void OnEnable()
        {
            instance = this;
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + "\n" + myLog;
            List<string> myLogList = myLog.Split('\n').ToList();
            if (myLogList.Count > 26)
            {
                myLog = string.Join("\n", myLogList.GetRange(0,25));
                
            }
            
        }

        public void ChangeStatus()
        {
            if (isActiveGUI == false)
            {
                isActiveGUI = true;
            }
            else
            {
                isActiveGUI = false;
            }
        }


        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            if(isActiveGUI)
            {
                myLog = GUI.TextArea(new Rect(10, 10, Screen.width/2, Screen.height/1.5f), myLog.ToString());
            }
        }
        //#endif
    }
}
