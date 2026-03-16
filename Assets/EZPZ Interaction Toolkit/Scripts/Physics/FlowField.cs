using UnityEngine;

public class FlowField : MonoBehaviour
{
    public float flowForce;
    public Transform direction;

    private void Start()
    {
        if (direction == null)
        {
            direction = transform;
        }
    }

    void OnTriggerStay(Collider c)
    {
        Rigidbody r = c.GetComponent<Rigidbody>();

        if(r != null )
        {
            if (direction != null)
            {
                r.AddForce(direction.forward * flowForce);
            }
        }
    }
}
