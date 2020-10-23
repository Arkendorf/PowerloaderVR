using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public OVRInput.Button gripButton;
    
    private Rigidbody grabbedRigidbody;
    private FixedJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedRigidbody && !OVRInput.Get(gripButton))
        {
            grabbedRigidbody = null;
            Destroy(joint);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            if (!grabbedRigidbody && OVRInput.Get(gripButton))
            {
                grabbedRigidbody = collision.rigidbody;
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = grabbedRigidbody;
            }
        }       
    }
}
