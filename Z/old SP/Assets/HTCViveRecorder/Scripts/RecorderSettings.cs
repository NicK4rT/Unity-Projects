using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveRecorder
{
    [Serializable]
    public struct RecorderActive
    {
        public string recorderName; 
        public IRecorder recorder;
        public bool isActive;
    }
    


    [CreateAssetMenu(fileName = "DefaultSettings", menuName = "ViveRecorderSettings", order = 1)]
    public class RecorderSettings : ScriptableObject
    {
        [Header("Settings for the ViveRecorder")]
        public string fileName = "Recordings"; // Will be changed
        public string fileDirectory = @"C:\Users\user\Documents\GitHub\VR-MachineTool-Teacher\Machine-Tool-Tutorial\Assets\Modern UI Pack\Scenes\";
        public bool isRecordingOn =true;
        public float sampleFrequency = 2.0f;


        // Add new Recordertype here to the list
        public RecorderActive[] recorderList = new RecorderActive[]
        {
            new RecorderActive(){recorderName = "TrajectoryRecorder", recorder = new TrajectoryRecorder(), isActive = false},
            new RecorderActive(){recorderName = "TobiiXRRecorder", recorder = new TobiiXRRecorder(), isActive = false}

        };

        public void ChangeValue(string name,bool value)
        {
            for(int i=0; i < recorderList.Length; ++i)
            {
                if(recorderList[i].recorderName == name)
                {
                    recorderList[i].isActive = value;
                }
            }
        }

        public void InitializeManagers()
        {
            for (int i = 0; i < recorderList.Length; ++i)
            {
                if (recorderList[i].isActive)
                {
                    recorderList[i].recorder.InitializeManager();
                }

            }
        }

        public void ShutOffManagers()
        {
            for (int i = 0; i < recorderList.Length; ++i)
            {
                if (recorderList[i].isActive)
                {
                    recorderList[i].recorder.ShutOffManager();
                }

            }


        }

        

    }
}
