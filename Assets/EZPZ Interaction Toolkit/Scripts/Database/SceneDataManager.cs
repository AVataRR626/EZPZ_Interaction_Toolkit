using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public FirebaseBridge myFirebaseBridge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(myFirebaseBridge == null)
            myFirebaseBridge = FirebaseBridge.Instance;
    }


    public void AddEvent(string eventString)
    {
        myFirebaseBridge.AddEvent(eventString);
    }

    public void PostToDatabase()
    {
        myFirebaseBridge.PostToDatabase();
    }
}
