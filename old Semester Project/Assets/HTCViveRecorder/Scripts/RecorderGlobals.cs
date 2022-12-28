using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveRecorder
{

    public class RecorderGlobals : Singleton<RecorderGlobals>
    {
        public RecorderSettings recorderSettings;

        private void Start()
        {
            DontDestroyOnLoad(this);
        }




    }
}
