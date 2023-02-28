using System.Collections.Generic;
using System;
using UnityEngine;
using Tobii.G2OM;
using Tobii.XR;
using System.Linq;


namespace ViveRecorder
{
    public class TobiiXRRecorder : IRecorder
    {

        // Variables for the saved file
        List<string> currentDataList = new List<string>();
        string content = "FocusedObject,LeftBlink,RightBlink,RayOrigin_x,RayOrigin_y,RayOrigin_z,RayDirection_x,RayDirection_y,RayDirection_z,ConvergenceDistance,foc_Objectpos_X,foc_Objectpos_Y,foc_Objectpos_Z";
        

        // Variables for the TobiiXR Eye-Tracking Asset
        public TobiiXR_TrackingSpace trackSpace = TobiiXR_TrackingSpace.World;
        public float convergeDistance = 0;
        public bool isLeftEyeBlinking = false;
        public bool isRightEyeBlinking = false;
        public Vector3 rayOrigin = new Vector3(0f, 0f, 0f);
        public Vector3 rayDirection = new Vector3(0f, 0f, 0f);
        public GameObject focusedGameObject;
        private UnityEngine.Object manager;

    //Variables for the SRancipal Eye-Tracking Asset (will be implemented later)
    /*
    private FocusInfo FocusInfo;
    private readonly float MaxDistance = 20;
    private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;


    public float distance;
    public Collider gazeCollider;
    public Vector3 focusPoint;
    */

        public void InitializeManager()
        {
       
           UnityEngine.Object obj = Resources.Load("TobiiXR/Custom_TobiiXR_Initializer"); //TobiiXR Initializer //Custom_TobiiXR_Initializer
            Debug.Log("Tobii Initialized");
            manager = UnityEngine.Object.Instantiate(obj);
           

        }

        public void ShutOffManager()
        {
            UnityEngine.Object.Destroy(manager);
        }


        public string GetRecorderHeader()
        {
            return content;
        }


        public bool GetRecordingData(out string data)
        {

            /*
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
           SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT)
            {
                data = null;
                return false;
            }


            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
            */

            currentDataList = new List<string>();
            TobiiXR_EyeTrackingData eyeTrackingData = TobiiXR.GetEyeTrackingData(trackSpace);
            isLeftEyeBlinking = eyeTrackingData.IsLeftEyeBlinking;
            isRightEyeBlinking = eyeTrackingData.IsRightEyeBlinking;
            //get current time
            //time = eyeTrackingData.Timestamp;
            if (isLeftEyeBlinking && isRightEyeBlinking)
            {
                currentDataList = Enumerable.Repeat("closed", 10).ToList();
                currentDataList[1] = "True";
                currentDataList[2] = "True";
            }
            else
            {
              
            if (eyeTrackingData.GazeRay.IsValid)
            {
                rayOrigin = eyeTrackingData.GazeRay.Origin;
                rayDirection = eyeTrackingData.GazeRay.Direction;


                if (eyeTrackingData.ConvergenceDistanceIsValid)
                {
                    convergeDistance = eyeTrackingData.ConvergenceDistance;
                }
                else
                {
                    convergeDistance = 0f;
                }

                    string focusedOjectName = "None";
                    Vector3 focusedObjectposition = Vector3.zero;

                // if an registered GameObject is focused, GameObject class can't be empty
                if (TobiiXR.FocusedObjects.Count > 0)
                {

                    List<FocusedCandidate> FocusedObjects = TobiiXR.FocusedObjects;
                    focusedGameObject = FocusedObjects[0].GameObject;
                    focusedOjectName = focusedGameObject.name;
                   focusedObjectposition = focusedGameObject.transform.position;


                }

                currentDataList.Add(focusedOjectName);
                currentDataList.Add(isLeftEyeBlinking.ToString());
                currentDataList.Add(isRightEyeBlinking.ToString());
                AddVectorToList(rayOrigin);
                AddVectorToList(rayDirection);
                currentDataList.Add(convergeDistance.ToString());
                AddVectorToList(focusedObjectposition);

            }
            else
            {
                currentDataList = Enumerable.Repeat("inactive", 10).ToList();
                
            }
            }

            data = "," + String.Join(",", currentDataList);
            return true;

        }

       

    void AddVectorToList(Vector3 vector)
    {

        currentDataList.Add(vector.x.ToString());
        currentDataList.Add(vector.y.ToString());
        currentDataList.Add(vector.z.ToString());


    }


        /*
        private void OnEnable(){

        // If SRancipal is not available (Works for both assets) 
        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            Debug.Log("Eye-Tracking does not work/ is not enabled");
            return;
        }
    }

    */


        /* SRansipal Release Function
    private void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }

    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
    }
    */
    }

}
