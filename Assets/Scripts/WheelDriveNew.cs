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
    private float steerKey;
    
    [SerializeField] float maxAngle = 30f;
    [SerializeField] float maxTorque = 3000f;
    [SerializeField] float brakeTorque = 30000f;
    [SerializeField] DriveType driveType;

    WheelCollider[] m_Wheels; 
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.neverAutoSwitchControlSchemes = true;
        driveType = DriveType.RearWheelDrive;

    }
    void Start()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            var wheel = m_Wheels[i];
        }
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
        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = 0f;
            }
        }
    }

    private void Steer_Performed(InputAction.CallbackContext obj)
    {
        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
             {
                wheel.steerAngle = obj.ReadValue<float>() * maxAngle;
            }
        }
    }


    private void Acceleration_Canceled(InputAction.CallbackContext obj)
    {
        foreach (WheelCollider wheel in m_Wheels)
        {
            wheel.motorTorque = 0f;


        }
    }

    private void Acceleration_Performed(InputAction.CallbackContext obj)
    {
        accelKey = obj.ReadValue<float>();
        foreach (WheelCollider wheel in m_Wheels)
        {

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = accelKey * maxTorque;
            }
            if (wheel.transform.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = accelKey * maxTorque;
            }
        }
    }
}
