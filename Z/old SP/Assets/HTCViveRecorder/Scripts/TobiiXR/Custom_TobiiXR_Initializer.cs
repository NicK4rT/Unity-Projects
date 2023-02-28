using Tobii.XR;
using UnityEngine;

// Customized TobiiXR Initializer (HTC Vive Pro Eye)
public class Custom_TobiiXR_Initializer : MonoBehaviour
{

    public TobiiXR_Settings Settings;

    private void Awake()
    {
        TobiiXR.Start(Settings);
        
    }

    public void OnDisable()
    {
        TobiiXR.Stop();
        Destroy(this.gameObject);
    }

   

}
