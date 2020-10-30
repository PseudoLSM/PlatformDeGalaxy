using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected Transform TargetObject; // Set object to follow (playerCharacter)

    protected Vector3 PositionOffset = new Vector3(0, 0, -1); // A offset for the camera if we ever need it

    [HideInInspector] public bool LookAtTarget = true; // Change if the camera is following or not (this might be useful for the smooth planet switching

    protected Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>(); // Gets position of the Camera
    }

    void LateUpdate() // Updates near the end of every frame
    {
        if (TargetObject)
        {
            myTransform.position = TargetObject.position + PositionOffset; // Actually moves the camera

            if (LookAtTarget) // Dont know why this is here, I don't think we need it.
            {
                myTransform.LookAt(TargetObject);
            }
        }
    }
}
