using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class WheelDrive : MonoBehaviour
{

    [SerializeField] float maxAngle = 30f;
    [SerializeField] float maxTorque = 300f;
    [SerializeField] float brakeTorque = 30000f;

    [SerializeField] float criticalSpeed = 5f;
    [SerializeField] int stepBelow = 5;
    [SerializeField] int stepAbove = 1;

    [SerializeField] DriveType driveType;
    WheelCollider[] m_Wheels;
    float handBrake, torque;
    public Vector2 angle;

    public InputActionAsset inputActions;
    InputActionMap gameplayActionMap;
    InputAction handBrakeInputAction;
    InputAction steeringInputAction;
    InputAction accelerationInputAction;

    void Awake()
    {
        gameplayActionMap = inputActions.FindActionMap("Driving");

        handBrakeInputAction = gameplayActionMap.FindAction("HandBrake");
        steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
        accelerationInputAction = gameplayActionMap.FindAction("Acceleration");

        handBrakeInputAction.performed += GetHandBrakeInput;
        handBrakeInputAction.canceled += GetHandBrakeInput;

        steeringInputAction.performed += GetAngleInput;
        steeringInputAction.canceled += GetAngleInput;

        accelerationInputAction.performed += GetTorqueInput;
        accelerationInputAction.canceled += GetTorqueInput;
    }


    void GetHandBrakeInput(InputAction.CallbackContext context)
    {
        handBrake = context.ReadValue<float>() * brakeTorque;
    }
    void GetAngleInput(InputAction.CallbackContext context)
    {
        angle = context.ReadValue<Vector2>() * maxAngle;
    }
    void GetTorqueInput(InputAction.CallbackContext context)
    {
        torque = context.ReadValue<float>() * maxTorque;
    }

    private void OnEnable()
    {
        handBrakeInputAction.Enable();
        steeringInputAction.Enable();
        accelerationInputAction.Enable();
    }

    private void OnDisable()
    {
        handBrakeInputAction.Disable();
        steeringInputAction.Disable();
        accelerationInputAction.Disable();
    }

    void Start()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            var wheel = m_Wheels[i];
        }
    }

    void Update()
    {
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbove);

        foreach (WheelCollider wheel in m_Wheels)
        {
            if(wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = angle.y;
            }
            if(wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }
            if(wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if (wheel.transform.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            
            }
        }
    }

