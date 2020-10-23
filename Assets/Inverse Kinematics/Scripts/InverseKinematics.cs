using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class InverseKinematics : MonoBehaviour {

	public Transform upperArm;
	public Transform forearm;
	public Transform hand;
	public Transform elbow;
	public Transform target;
	[Space(20)]
	public Vector3 uppperArm_OffsetRotation;
	public Vector3 forearm_OffsetRotation;
	public Vector3 hand_OffsetRotation;
	[Space(20)]
	public bool handMatchesTargetRotation = true;
    public bool stretch = true;
	[Space(20)]
	public bool debug;

	float angle;
	float upperArm_Length;
	float forearm_Length;
	float arm_Length;
	float targetDistance;
	float adjacent;

    private float defaultUpperArmLength;
    private float defaultForearmLength;
    private float defaultArmLength;

    private float armScale;
    private Vector3 defaultUpperArmScale;

	// Use this for initialization
	void OnEnable() {
        defaultUpperArmLength = Vector3.Distance(upperArm.position, forearm.position);
        defaultForearmLength = Vector3.Distance(forearm.position, hand.position);
        defaultArmLength = defaultUpperArmLength + defaultForearmLength;

        defaultUpperArmScale = upperArm.localScale;
    }

    private void OnDisable()
    {
        upperArm.localScale = defaultUpperArmScale;
    }

    // Update is called once per frame
    void LateUpdate () {
		if(upperArm != null && forearm != null && hand != null && elbow != null && target != null){
            // Update scale        
            if (stretch)
            {
                // Get total necessary arm length
                float dist = Vector3.Distance(upperArm.position, target.position);
                // Get necessary arm scale to reach target
                armScale = Mathf.Max(1f, (dist / defaultArmLength));
            }
            else
            {
                armScale = 1;
            }
            // Update scale
            upperArm.localScale = new Vector3(defaultUpperArmScale.x, defaultUpperArmScale.y, defaultUpperArmScale.z * armScale);



            upperArm.LookAt(target, elbow.position - upperArm.position);
            upperArm.Rotate(uppperArm_OffsetRotation);

            Vector3 cross = Vector3.Cross(elbow.position - upperArm.position, forearm.position - upperArm.position);


            upperArm_Length = defaultUpperArmLength * armScale;
            forearm_Length = defaultForearmLength * armScale;
            arm_Length = upperArm_Length + forearm_Length;
            targetDistance = Vector3.Distance(upperArm.position, target.position);
            targetDistance = Mathf.Min(targetDistance, arm_Length - arm_Length * 0.001f);

            adjacent = ((upperArm_Length * upperArm_Length) - (forearm_Length * forearm_Length) + (targetDistance * targetDistance)) / (2 * targetDistance);

            angle = Mathf.Acos(adjacent / upperArm_Length) * Mathf.Rad2Deg;

            upperArm.RotateAround(upperArm.position, cross, -angle);

            forearm.LookAt(target, cross);
            forearm.Rotate(forearm_OffsetRotation);

            if (handMatchesTargetRotation)
            {
                hand.rotation = target.rotation;
                hand.Rotate(hand_OffsetRotation);
            }

            if (debug){
				if (forearm != null && elbow != null) {
					Debug.DrawLine (forearm.position, elbow.position, Color.blue);
				}

				if (upperArm != null && target != null) {
					Debug.DrawLine (upperArm.position, target.position, Color.red);
				}
			}				
		}		
	}

	void OnDrawGizmos(){
		if (debug) {
			if(upperArm != null && elbow != null && hand != null && target != null && elbow != null){
				Gizmos.color = Color.gray;
				Gizmos.DrawLine (upperArm.position, forearm.position);
				Gizmos.DrawLine (forearm.position, hand.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (upperArm.position, target.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (forearm.position, elbow.position);
			}
		}
	}

}
