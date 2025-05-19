using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;


public class FirebaseBridge : MonoBehaviour
{
    public static FirebaseBridge Instance;

    [Header("Experiment Data")]
    public string experimentString = "mewowow";
    public string participantID = "default";    

    [Header("System")]
    public string firebaseLink = "https://your-url.firebaseio.com/";
    public string fullPostLink;
    public bool post2Database = true;
    public List<string> eventLogCache;
    public bool verboseEventLog = false;    
    public float utcOffset = 11;
    public ExperimentData ed;//experimental data
    

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);            
        }
        else
            Destroy(gameObject);


        TryInitED();

        if (!post2Database)
            Debug.LogError("WARNING: FirebaseBridge: Not Posting to Database");

    }

    void TryInitED()
    {
        if (ed == null)
        {
            ed = new ExperimentData();
            //ed.timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            DateTime timeStamp = System.DateTime.UtcNow;
            timeStamp.AddHours(utcOffset);
            ed.timeStamp = timeStamp.ToString("yyyy-MM-dd-HH-mm-ss");
            ed.participantID = participantID;
        }
        else
        {
            if (ed.timeStamp.Length == 0)
                ed.timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        GenerateFilename();
    }

    public void GenerateFilename()
    {
        fullPostLink = firebaseLink + experimentString + "_" + ed.timeStamp + "_" + participantID + ".json";
        //fullPostLink = firebaseLink + ed.timeStamp + "_" + experimentString + ".json";
    }

    public void PostToDatabase()
    {
        if (post2Database)
        {
            GenerateFilename();

            Debug.Log(fullPostLink);
            RestClient.Put(fullPostLink, ed);
        }
        else
        {
            Debug.LogError("WARNING: FirebaseBridge: Not Posting to Database");
        }
    }

    public void AddEvent(string eventString)
    {
        string timeStampedEventString = Time.time.ToString() + " ::" + eventString;
        eventLogCache.Add(timeStampedEventString);
        ed.eventLog.Add(timeStampedEventString);
    }
}
