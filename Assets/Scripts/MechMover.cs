using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMover : MonoBehaviour
{
    public float joystickThreshold = .1f;
    public float moveSpeed = 1;
    public float rotationSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 positionChange = new Vector3(TranslateValue(moveJoystick.x, joystickThreshold), 0, TranslateValue(moveJoystick.y, joystickThreshold)) * moveSpeed * Time.deltaTime;
        transform.position += transform.rotation * positionChange;
  
        Vector2 rotJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        transform.Rotate(Vector3.up, TranslateValue(rotJoystick.x, joystickThreshold) * rotationSpeed * Time.deltaTime);
    }

    float TranslateValue(float value, float threshold)
    {
        if (value > threshold)
        {
            return (value - threshold) / (1 - threshold);
        }
        else if (value < -threshold)
        {
            return (value + threshold) / (1 - threshold);
        }
        else
        {
            return 0;
        }
    }
}
