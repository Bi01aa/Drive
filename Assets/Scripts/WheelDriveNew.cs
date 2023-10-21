using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

enum DriveType { RearWheelDrive, FrontWheelDrive, AllWheelDrive }
public class WheelDriveNew : MonoBehaviour
{
    private PlayerInput playerInput = null;
    public GameObject playerCar = null;
    private float accelKey;
    public GameObject mesh;
    
    
    [SerializeField] float maxAngle = 30f;
    [SerializeField] float maxTorque = 3000f;
    [SerializeField] float brakeTorque = 30000f;
    [SerializeField] DriveType driveType;

    WheelCollider wheelCollider = null;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.neverAutoSwitchControlSchemes = true;
        driveType = DriveType.RearWheelDrive;

    }
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();

    }
    private void OnDisable()
    {
        playerInput.actions["Acceleration"].performed -= Acceleration_Performed;
        playerInput.actions["Acceleration"].canceled -= Acceleration_Canceled;
        playerInput.actions["SteeringAngle"].performed -= Steer_Performed;
        playerInput.actions["SteeringAngle"].canceled -= Steer_Canceled;
    }
    private void OnEnable()
    {
        playerInput.actions["Acceleration"].performed += Acceleration_Performed;
        playerInput.actions["Acceleration"].canceled += Acceleration_Canceled;
        playerInput.actions["SteeringAngle"].performed += Steer_Performed;
        playerInput.actions["SteeringAngle"].canceled += Steer_Canceled;
    }

    private void Steer_Canceled(InputAction.CallbackContext obj)
    {
        if (wheelCollider.transform.parent.localPosition.z > 0)
        {
            wheelCollider.steerAngle = 0f;
        }
    }

    private void Steer_Performed(InputAction.CallbackContext obj)
    {
      
        if (wheelCollider.transform.parent.localPosition.z > 0)
        {
            wheelCollider.steerAngle = obj.ReadValue<float>() * maxAngle;
        }
        
    }


    private void Acceleration_Canceled(InputAction.CallbackContext obj)
    {
        
            wheelCollider.motorTorque = 0f;


        
    }

    private void Acceleration_Performed(InputAction.CallbackContext obj)
    {
        accelKey = obj.ReadValue<float>();
        
        

            if (wheelCollider.transform.parent.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheelCollider.motorTorque = accelKey * maxTorque;
            }
            if (wheelCollider.transform.parent.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheelCollider.motorTorque = accelKey * maxTorque;
            }
        
    }

    private void UpdateWheel()
    {
        Vector3 vect;
        Quaternion quat;
        wheelCollider.GetWorldPose(out vect, out quat);
                            
        MeshRenderer meshR = mesh.GetComponent<MeshRenderer>();
        meshR.transform.position = vect;
        meshR.transform.rotation = quat;
    }
    private void FixedUpdate()
    {
        UpdateWheel();
    }
}
