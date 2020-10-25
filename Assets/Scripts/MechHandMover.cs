using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHandMover : MonoBehaviour
{
    //[System.Serializable]
    //public class ForceSettings
    //{
    //    public float velocityScale = 2.5f;
    //    public float maxVelocity = 15.0f;
    //    public float maxForce = 40.0f;
    //    public float gain = 5f;
    //}

    public Transform mechBody;
    public Transform handAnchor;
    public float scale = 2;
    [Space(10)]
    public float forceScale = 10;
    public float maxForce = 8;
    [Space(10)]
    public float torqueScale = 1;
    public float maxTorque = 8;



    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = maxTorque;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Set the goal position of this hand
        Vector3 targetPos = mechBody.rotation * handAnchor.localPosition * scale;

        Vector3 dist = targetPos - mechBody.rotation * transform.localPosition;
        Vector3 force = dist * forceScale;
        if (force.magnitude > maxForce)
        {
            force = Vector3.Normalize(force) * maxForce;
        }
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force);


        //Find the rotation difference in eulers
        Quaternion diff = Quaternion.Inverse(rigidbody.rotation) * handAnchor.rotation;
        Vector3 eulers = OrientTorque(diff.eulerAngles);
        Vector3 torque = eulers * torqueScale;
        //put the torque back in body space
        torque = rigidbody.rotation * torque;

        if (torque.magnitude > maxTorque)
        {
            torque = Vector3.Normalize(torque) * maxTorque;
        }
        //just zero out the current angularVelocity so it doesnt interfere
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddTorque(torque);
    }

    //private Vector3 GetForce(Vector3 dist)
    //{
    //    Vector3 tgtVel = Vector3.ClampMagnitude(moveSettings.velocityScale * dist, moveSettings.maxVelocity);
    //    // calculate the velocity error
    //    Vector3 error = tgtVel - rigidbody.velocity;
    //    // calc a force proportional to the error (clamped to maxForce)
    //    Vector3 force = Vector3.ClampMagnitude(moveSettings.gain * error, moveSettings.maxForce);

    //    return force;
    //}



    private Vector3 OrientTorque(Vector3 torque)
    {
        // Quaternion's Euler conversion results in (0-360)
        // For torque, we need -180 to 180.

        return new Vector3
        (
        torque.x > 180f ? torque.x - 360f : torque.x,
        torque.y > 180f ? torque.y - 360f : torque.y,
        torque.z > 180f ? torque.z - 360f : torque.z
        );
    }
}
