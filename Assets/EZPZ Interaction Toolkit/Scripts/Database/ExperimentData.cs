using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExperimentData
{
    public string participantID;
    public string timeStamp;
    public List<string> eventLog;

    public ExperimentData()
    {
        eventLog = new List<string>();        
        timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-s");
    }

}
