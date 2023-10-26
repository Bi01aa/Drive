using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public enum CameraState {ChaseFar, ChaseClose, FirstPerson, Bonnet, Nose}


public class CameraController : MonoBehaviour
{
    
    public Camera cam;
    PlayerInput playerInput;
    private int camIndex = 0;
    private Tuple<Vector3, Vector3> CamPos;
    // Start is called before the first frame update
    private void Awake()
    {
       
        playerInput = GetComponentInParent<PlayerInput>();
    }
    private void OnDisable()
    {
        playerInput.actions["CameraSwitch"].started -= CameraController_started;
        
    }
    private void OnEnable()
    {
        playerInput.actions["CameraSwitch"].started += CameraController_started;
        
    }

    


    private void CameraController_started(InputAction.CallbackContext obj)
    {
        if (camIndex < 4)
        {
            camIndex += 1;
        }
        else
        {
            camIndex = 0;
        }
        UpdateCam(ref camIndex, out CamPos);
        cam.transform.localPosition = CamPos.Item1;
        cam.transform.localRotation = Quaternion.Euler(CamPos.Item2);


    }

    private void UpdateCam(ref int camIndex, out Tuple<Vector3, Vector3> pos)
    {
        pos = new Tuple<Vector3, Vector3>(new Vector3(0, 3.4f, -6), new Vector3(12, 0, 0));
        switch (camIndex)
        {

            case 0:
                pos = new Tuple<Vector3, Vector3>(new Vector3(0, 3.4f, -6), new Vector3(12, 0, 0));
                break;
            case 1:
                pos = new Tuple<Vector3, Vector3>(new Vector3(0, 2.5f, -3.5f), new Vector3(12, 0, 0));
                break;
            case 2:
                pos = new Tuple<Vector3, Vector3>(new Vector3(-0.4f, 1.25f, 0), new Vector3(7, 0, 0));
                break;
            case 3:
                pos = new Tuple<Vector3, Vector3>(new Vector3(0, 1.25f, 1.25f), new Vector3(3.25f, 0, 0));
                break;
            case 4:
                pos = new Tuple<Vector3, Vector3>(new Vector3(0, 0.75f, 2), new Vector3(3.25f, 0, 0));
                break;

        }
        return;
    }
    
}
    // Update is called once per frame
