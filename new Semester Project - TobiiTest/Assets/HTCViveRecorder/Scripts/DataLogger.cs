using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ViveRecorder {

    public class DataLogger : Singleton<DataLogger>
    {
        public RecorderSettings recorderSettings;
        private string filePath;
        public enum LoggingState {logging,not_logging};
        public LoggingState state = LoggingState.not_logging;

        StreamWriter csvWriter;

        public List<IRecorder> recorderList = new List<IRecorder>();
        string header = "TimeSinceStartup,";

        public Toggle isRecordingToggle;
        private float currentTime = 0;
        

        // Start is called before the first frame update
        void Start()
        {
            if (recorderSettings == null)
            {
                if(RecorderGlobals.Instance != null)
                {
                    recorderSettings = RecorderGlobals.Instance.recorderSettings;
                    
                }
                else
                {
                    enabled = false;
                }
                
            }


            if(isRecordingToggle != null)
            {
                if(recorderSettings != null)
                {
                    isRecordingToggle.isOn = recorderSettings.isRecordingOn;
                }
                
                isRecordingToggle.onValueChanged.AddListener(delegate { ChangeLoggingState(isRecordingToggle.isOn); });
                isRecordingToggle.onValueChanged.Invoke(isRecordingToggle.isOn);
            }
            

            if (recorderSettings != null && recorderSettings.isRecordingOn)
            {
                if(isRecordingToggle != null)
                {
                    isRecordingToggle.gameObject.SetActive(true);
                }
                

                int numRecorderOn = 0;

                recorderSettings.InitializeManagers();


                foreach (RecorderActive rec in recorderSettings.recorderList)
            {
                if (rec.isActive)
                {
                    
                    recorderList.Add(rec.recorder);
                    
                    print(rec.recorderName + " is Active");
                    header = header + rec.recorder.GetRecorderHeader();
                    numRecorderOn += 1;
                   
                }

            }
            
            if(numRecorderOn > 0)
                {
                    csvWriter = new StreamWriter(recorderSettings.fileDirectory + "/" + recorderSettings.fileName + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");
                    csvWriter.WriteLine(header);
                    state = LoggingState.logging;
                    StartCoroutine(startSampling());
                    
                }
            }
            
        }

        private void OnDisable()
        {
            if(csvWriter != null)
            {
                csvWriter.Close();
                print("Datalogger closed csv File");
            }

            if(recorderSettings != null)
            {
                recorderSettings.ShutOffManagers();
            }
            
            
        }


        IEnumerator startSampling()
        {
            WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(1f / recorderSettings.sampleFrequency);

            

            while (true)
            {
                currentTime = Time.timeSinceLevelLoad;

                if (state == LoggingState.logging)
                {
                    SampleNow();
                }
                
                yield return waitTime;
            }
        }
        


        public void SampleNow()
        {

            string completeLine = currentTime.ToString();
            string dataString = null;
            for(int i = 0; i < recorderList.Count; i++)
            {
                bool isActiveStatus = recorderList[i].GetRecordingData(out dataString);
                if (isActiveStatus)
                {
                    completeLine += dataString ;
                }

                
            }

            csvWriter.WriteLine(completeLine);


        }

        public void ChangeLoggingState(bool isLoggingOn)
        {

            if(!isLoggingOn)
            {
                state = LoggingState.not_logging;
            }
            else
            {
                state = LoggingState.logging;
            }
        }
        

    }
}
