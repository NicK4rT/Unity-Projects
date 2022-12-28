using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecorder
{
    void InitializeManager();
    void ShutOffManager();
    string GetRecorderHeader();
    bool GetRecordingData(out string logging_data);

}
