using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

/// <summary>
/// Writes transforms of two hands and the head mounted display into a .txt file in the Asset/RecordedTrajectories folder. 
/// WARNING: Textfile is overwritten each time the programm runs / the scene is loaded!!!
/// </summary>
namespace ViveRecorder
{


public class TrajectoryRecorder : IRecorder
    {
        static string content = "Lhand_xPos,Lhand_yPos,Lhand_zPos,Lhand_wQuat,Lhand_xQuat,Lhand_yQuat,Lhand_zQuat,Lhandxrot,Lhandyrot,Lhandzrot," +
                                "Rhand_xPos,Rhand_yPos,Rhand_zPos,Rhand_wQuat,Rhand_xQuat,Rhand_yQuat,Rhand_zQuat,Rhandxrot,Rhandyrot,Rhandzrot," +
                                "HMD_xPos,HMD_yPos,HMD_zPos,HMD_wQuat,HMD_xQuat,HMD_yQuat,HMD_zQuat,HMDxrot,HMDyrot,HMDzrot," +
                                "vrtracker_xPos,vrtracker_yPos,vrtracker_zPos,vrtracker_wQuat,vrtracker_xQuat,vrtracker_yQuat,vrtracker_zQuat,vrtracker_xrot,vrtracker_yrot,vrtracker_zrot," +
                                "rtracker_xPos,rtracker_yPos,rtracker_zPos,rtracker_wQuat,rtracker_xQuat,rtracker_yQuat,rtracker_zQuat,rtracker_xrot,rtracker_yrot,rtracker_zrot,";

        public bool isActive;

        Transform m_rHandTransform;
        Transform m_lHandTransform;
        Transform m_HMDtransfrom;
        Transform m_lshouldertransform;
        Transform m_rshouldertransform;

        
        Player manager = null;

        public void InitializeManager()
        {
            try
            {
                manager = Player.instance;
            }
            catch
            {
                Debug.Log("No Player instance assigned (SteamVR)");
            }
                
            
        }


        public void ShutOffManager()
        {
            if(manager != null)
            {
                UnityEngine.Object.Destroy(manager.gameObject);
            }
            
        }

        public string GetRecorderHeader()
        {
                return content;
        }

        public bool GetRecordingData(out string data)
        {
            data = " ";
            if(manager != null)
            {
                m_HMDtransfrom = Player.instance.hmdTransform;
                m_lHandTransform = Player.instance.leftHand.transform;
                m_rHandTransform = Player.instance.rightHand.transform;
                //leftShoulder = tracker 2 = rtracker = LHR-20099559
                m_lshouldertransform = Player.instance.leftShoulder.transform;
                //rightShoulder = tracker 1 = vrtracker = LHR-C89590BE
                m_rshouldertransform = Player.instance.rightShoulder.transform;
                //m_lshouldertransform = LeftShoulder.transform;
                //m_rshouldertransform = RightShoulder.transform;
                //m_lshouldertransform = Player.FindGameObjectWithTag("LeftShoulder").transform;
                //m_rshouldertransform = Player.FindGameObjectWithTag("RightShoulder").transform;

                string lHandTransformString = GetTransformAsString(m_lHandTransform);
                string rHandTransformString = GetTransformAsString(m_rHandTransform);
                string HMDTransformString = GetTransformAsString(m_HMDtransfrom);
                string lshoulderTransformString = GetTransformAsString(m_lshouldertransform);
                string rshoulderTransformString = GetTransformAsString(m_rshouldertransform);
                data = "," + lHandTransformString + "," + rHandTransformString + "," + HMDTransformString + "," + lshoulderTransformString + "," + rshoulderTransformString;
                

            }
            else
            {
                List<string> currentDataList = Enumerable.Repeat("inactive", 21).ToList();
                data = "," + String.Join(",", currentDataList);
            }

            return true;


        }



            string GetTransformAsString(Transform GOTransform)
            {

            //get all components of Transform as String
            string xCoord = GOTransform.position.x.ToString();
            string yCoord = GOTransform.position.y.ToString();
            string zCoord = GOTransform.position.z.ToString();
            string wQuat = GOTransform.rotation.w.ToString();
            string xQuat = GOTransform.rotation.x.ToString();
            string yQuat = GOTransform.rotation.y.ToString();
            string zQuat = GOTransform.rotation.z.ToString();
            string xrot = GOTransform.transform.eulerAngles.x.ToString();
            string yrot = GOTransform.transform.eulerAngles.y.ToString();
            string zrot = GOTransform.transform.eulerAngles.z.ToString();

            //Combine individual strings into one string
            string currentTransformAsString = xCoord + "," + yCoord + "," + zCoord + "," + wQuat + "," + xQuat + "," + yQuat + "," + zQuat + "," + xrot + "," + yrot + "," + zrot ;
            //string currentTransformAsString = xCoord + "," + yCoord + "," + zCoord + "," + xrot + "," + yrot + "," + zrot;

            //Return all components of the current transform as one string (separated by commas)
            return currentTransformAsString;
            }

        }

}
