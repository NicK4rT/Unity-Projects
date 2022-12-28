using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ViveRecorder;


// Basic DataLogger for a test run (not finished yet)
public class TestDataLogger : MonoBehaviour
{

    public RecorderSettings recorderSettings;
    private float samplingFrequenzy = 2;

    StreamWriter csvWriter;

    public List<IRecorder> recorderList = new List<IRecorder>();
    string header = null;
    private float currentTime = 0;
    private float testTime = 10;
    private string fileName = "FunctionTest.csv";
    private float startTime = 0;


    // Start is called before the first frame update
    void Start()
    {


        if (recorderSettings == null)
        {
            if (RecorderGlobals.Instance != null)
            {
                recorderSettings = RecorderGlobals.Instance.recorderSettings;
            }
            else
            {
                enabled = false;
            }

        }


       

        if (recorderSettings != null && recorderSettings.isRecordingOn)
        {

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

            if (numRecorderOn > 0)
            {
                csvWriter = new StreamWriter(fileName);
                csvWriter.WriteLine("Time: " + System.DateTime.Now.ToString("yyyyMMdd_HHmm"));
                csvWriter.WriteLine(header);
                startTime = Time.time;
                InvokeRepeating("SampleNow", 1.0f, 1f / samplingFrequenzy);
            }
            else
            {
                enabled = false;
            }


        }





    }

    private void OnDisable()
    {
        if (csvWriter != null)
        {
            csvWriter.Close();
            System.Diagnostics.Process.Start(fileName);
            
        }

        print("TestDatalogger closed csv File");
    }





    public void SampleNow()
    {
        currentTime = Time.time;
        if (currentTime - startTime < testTime)
        {
            

        
        
        string completeLine = currentTime.ToString();
        string dataString = null;
        for (int i = 0; i < recorderList.Count; i++)
        {
            bool isActiveStatus = recorderList[i].GetRecordingData(out dataString);
            if (isActiveStatus)
            {
                completeLine += dataString;
            }


        }


        csvWriter.WriteLine(completeLine);
        print(currentTime-startTime);
        }
        else
        {
            Debug.Log("Test is over");
            CancelInvoke();
            enabled = false;
        }


    }

   


}
