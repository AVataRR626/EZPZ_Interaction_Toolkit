//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 18 Apr 2025
using UnityEngine;

[RequireComponent (typeof(SimpleTeleporter))]  
public class TeleportItemTrigger : InteractableTrigger
{
    [Header("Teleport Settings")]
    public bool autoTeleport = true;

    [Header("System Stuff (Usually Don't Touch")]
    public SimpleTeleporter teleporter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(teleporter == null)
            teleporter = GetComponent<SimpleTeleporter>();

        if (autoTeleport)
            onTriggerEnter.AddListener(Teleport);
    }

    public void Teleport()
    {
        Movable m = subject.GetComponent<Movable>();

        if (m != null)
            m.ForceDrop();

        teleporter.subject = subject.transform;
        teleporter.Teleport();
    }
}
