using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.SceneManagement;
using ViveRecorder;
using System;
using SimpleFileBrowser;
using System.IO;

public class WaitingRoomGUIEventHandler : MonoBehaviour
{
    // Recording parameters
    public bool isRecordingOn = false;
    public string filePath = "C:";
    public float samplingFrequency = 1;
    public string selectedScene = " ";
    public string fileName = " ";

    // Objects for recording
    public RecorderGlobals globals { get; private set; }
    public RecorderSettings recorderSettings { get; private set; }
    RecorderActive[] list;

    // UI elements
    public CustomDropdown sceneDropDown;
    public DropdownMultiSelect recorderDropdown;
    public GameObject recordField;
    public GameObject togglePrefab;

    
    private void Awake()
    {
        filePath = UnityEngine.Application.dataPath +"/Recordings";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        fileName = string.Format("Recordings_",DateTime.Now);
        if (RecorderGlobals.Instance == null)
        {
            GameObject gObject = new GameObject("RecorderGlobals");
            globals = gObject.AddComponent<RecorderGlobals>();
            
        }
        else
        {
            globals = RecorderGlobals.Instance;
        }
        recorderSettings = ScriptableObject.CreateInstance<RecorderSettings>();

        AddScenesToDropDown();
        AddRecorderTypesToDropDown();

    }



    public void AddRecorderTypesToDropDown()
    {
        list = recorderSettings.recorderList;

        foreach (RecorderActive rec in list)
        {


            GameObject go = Instantiate(togglePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.transform.SetParent(recordField.transform, false);
            go.name = rec.recorderName;
            go.GetComponentInChildren<Text>().text = rec.recorderName;

            Toggle itemToggle = go.GetComponent<Toggle>();

            itemToggle.onValueChanged.AddListener(delegate { UpdateRecorderList(itemToggle); });
            itemToggle.onValueChanged.Invoke(itemToggle.isOn);
        }
    }
        
    
    
    void UpdateRecorderList(Toggle toggle) 
    {   
        recorderSettings.ChangeValue(toggle.gameObject.name, toggle.isOn);
        recorderSettings.recorderList = list;

    }


    public void AddScenesToDropDown()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

            
        for (int i = 1; i < sceneCount; ++i)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            sceneDropDown.SetItemTitle(name);
            sceneDropDown.CreateNewItem();
        }

        sceneDropDown.SetupDropdown();

        selectedScene = sceneDropDown.dropdownItems[sceneDropDown.selectedItemIndex].itemName;

    }


    public void ChangedSelectedScene(CustomDropdown drop)
    {
        selectedScene = drop.dropdownItems[drop.selectedItemIndex].itemName;
    }


    public void recordEnableToggleClick(Toggle toggle)
    {
        isRecordingOn = toggle.isOn;
        recorderSettings.isRecordingOn = isRecordingOn;
        
    }

    public void SetFrequenzy(Slider slider)
    {
        string value = slider.value.ToString("F1");
        samplingFrequency = float.Parse(value);
    }


    
    public void PathChanged()
    {
        FileBrowser.ShowLoadDialog((paths) => { Debug.Log("Selected: " + paths[0]); filePath = paths[0] + "/"; },
                                () => { Debug.Log("Canceled"); },
                                  true, false, null, "Select Folder", "Select");
    }

    public void FilenameChange(TMP_InputField field)
    {
        fileName = field.text;
        
    }

    public void StartButtonClicked()
    {
        
        recorderSettings.sampleFrequency = samplingFrequency;
        recorderSettings.fileDirectory = filePath;
        recorderSettings.fileName = fileName;
        globals.recorderSettings = recorderSettings;
        globals.recorderSettings.isRecordingOn = isRecordingOn;
        SceneManager.LoadScene(selectedScene);
        
    }

    // will be implemented later (Function Test)

        /*
    public void TestButtonClicked()
    {
        Debug.Log("TestButtonClicked");
        testDataLogger.recorderSettings = recorderSettings;
        testDataLogger.enabled = true;
    }
    */
}





