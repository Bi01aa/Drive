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
    
    private float accelKey;
    private float brakeKey = 0;
    public GameObject mesh;
    
    
    private float maxAngle = 30f;
    private float steerAngle = 30f;
    private float maxTorque = 1000f;
    private float brakeTorque = 1700f;
    private float brakeBias = 0.6f;
    private Rigidbody body;
    private DriveType driveType;

    WheelCollider wheelCollider = null;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.neverAutoSwitchControlSchemes = true;
        driveType = DriveType.RearWheelDrive;
        body = GetComponentInParent<Rigidbody>();

    }
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();

    }
    private void OnDisable()
    {
        //please unsubscribe all to prevent memory leaks
        playerInput.actions["Acceleration"].performed -= Acceleration_Performed;
        playerInput.actions["Acceleration"].canceled -= Acceleration_Canceled;
        playerInput.actions["SteeringAngle"].performed -= Steer_Performed;
        playerInput.actions["SteeringAngle"].canceled -= Steer_Canceled;
        playerInput.actions["Brake"].performed -= Brake_Performed;
        playerInput.actions["Brake"].canceled -= Brake_Canceled;
    }
    private void OnEnable()
    {
        //add new input events here
        playerInput.actions["Acceleration"].performed += Acceleration_Performed;
        playerInput.actions["Acceleration"].canceled += Acceleration_Canceled;
        playerInput.actions["SteeringAngle"].performed += Steer_Performed;
        playerInput.actions["SteeringAngle"].canceled += Steer_Canceled;
        playerInput.actions["Brake"].performed += Brake_Performed;
        playerInput.actions["Brake"].canceled += Brake_Canceled; 
    }

    private void brakeUpdate(float brake)
    {
        
        if (wheelCollider.rotationSpeed > 10f)
        {
            if (wheelCollider.transform.parent.localPosition.z < 0)
            {
                wheelCollider.motorTorque = brake * brakeTorque * (1f - brakeBias);
            }
            if (wheelCollider.transform.parent.localPosition.z > 0)
            {
                wheelCollider.motorTorque = brake * brakeTorque * brakeBias;
            }
        }
        else if (wheelCollider.rotationSpeed < 10f)
        {
            if (wheelCollider.transform.parent.localPosition.z < 0)
            {
                wheelCollider.motorTorque = (brake * brakeTorque * (1f - brakeBias)) * -1f;
            }
            if (wheelCollider.transform.parent.localPosition.z > 0)
            {
                wheelCollider.motorTorque = (brake * brakeTorque * brakeBias) * -1f;
            }
        }
        else
        {
            wheelCollider.motorTorque = 0f;
            wheelCollider.rotationSpeed = 0f;
        }
    }


    private void Brake_Performed(InputAction.CallbackContext obj)
    {
        brakeKey = obj.ReadValue<float>() * -1f;
    }

    private void Brake_Canceled(InputAction.CallbackContext obj)
    {
        brakeKey = 0f;
        Debug.Log("cancelled");
        wheelCollider.motorTorque = 0f;
    }

    //resets steering angle when released
    private void Steer_Canceled(InputAction.CallbackContext obj)
    {
        if (wheelCollider.transform.parent.localPosition.z > 0)
        {
            wheelCollider.steerAngle = 0f;
        }
    }

    //steering angle controller
    private void Steer_Performed(InputAction.CallbackContext obj)
    {
      
        if (wheelCollider.transform.parent.localPosition.z > 0)
        {
            if (wheelCollider.rotationSpeed < 2000f)
            {
                wheelCollider.steerAngle = obj.ReadValue<float>() * maxAngle;
                
            }
            else
            {
                wheelCollider.steerAngle = obj.ReadValue<float>() * maxAngle / (body.velocity.magnitude/10);
            }

        }
        Debug.Log(wheelCollider.rotationSpeed + " " + wheelCollider.steerAngle);
    }

    //resets acceleration when released
    private void Acceleration_Canceled(InputAction.CallbackContext obj)
    {
        
            wheelCollider.motorTorque = 0f;


        
    }

    private void Acceleration_Performed(InputAction.CallbackContext obj)
    {
        accelKey = obj.ReadValue<float>();
        
        // changed the two axis controller to be one axis

            if (wheelCollider.transform.parent.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheelCollider.motorTorque = accelKey * maxTorque;
            }
            if (wheelCollider.transform.parent.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheelCollider.motorTorque = accelKey * maxTorque;
            }
            // elseif required here for AWD
        
    }

    private void UpdateWheel()
    {
        // moves the mesh to match the wheel collider

        Vector3 vect;
        Quaternion quat;
        wheelCollider.GetWorldPose(out vect, out quat);
                            
        MeshRenderer meshR = mesh.GetComponent<MeshRenderer>();
        meshR.transform.position = vect;
        meshR.transform.rotation = quat;
    }
    private void FixedUpdate()
    {
        steerAngle = maxAngle / (wheelCollider.rpm/360);
        UpdateWheel();
        if (brakeKey < 0f)
        {
            brakeUpdate(brakeKey); Debug.Log(brakeKey);
        }
    }
}
