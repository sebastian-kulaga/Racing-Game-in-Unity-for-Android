using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour
{

    public float turnspeed = 20f;
    public WheelCollider wheel_01; //przednie kola
    public WheelCollider wheel_04;
    public WheelCollider wheel_02;
    public WheelCollider wheel_03;
    public float targetSteerAngle = 0;
    public float maxMotorTorque = 140f;
    public float MotorTorque = 0;
    public float currentSpeed;
    public float currentBrake;
    public float maxSpeed = 20f;
    public Vector3 centerOfMass;
    public float maxBrakeTorque = 10000000f;
    public float pitch;
    public float vol;
    public bool braking;
    public int brakeforce=100000;

    // Use this for initialization
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        Braking();
        LerpToSteerAngle();
        EngineSound();
    }
    private void ApplySteer()
    {

        float angle = CrossPlatformInputManager.GetAxis("Horizontal");
        //Debug.Log(angle);
        targetSteerAngle = angle*turnspeed;


    }
    private void Drive()
    {
        bool engine = CrossPlatformInputManager.GetButton("ENGINE");
        //MotorTorque = maxMotorTorque;
        currentSpeed = 2 * Mathf.PI * wheel_01.radius * wheel_01.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed && wheel_03.brakeTorque == 0 && engine)
        {
            wheel_01.motorTorque = maxMotorTorque;
            wheel_04.motorTorque = maxMotorTorque;
        }
        else
        {
            wheel_01.motorTorque = 0;
            wheel_04.motorTorque = 0;
        }

    }

    private void Braking()
    {

        bool brake = CrossPlatformInputManager.GetButton("BRAKE");
        if (brake)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * Time.deltaTime * (-brakeforce));
            wheel_02.brakeTorque = maxBrakeTorque;
            wheel_03.brakeTorque = maxBrakeTorque;
            wheel_01.motorTorque = 0;
            wheel_04.motorTorque = 0;
            currentBrake = wheel_02.brakeTorque;
        }
        else
        {
            wheel_02.brakeTorque = 0;
            wheel_03.brakeTorque = 0;
        }

    }


private void LerpToSteerAngle()
{
        wheel_01.steerAngle = Mathf.Lerp(wheel_01.steerAngle, targetSteerAngle, Time.deltaTime * turnspeed);
        wheel_04.steerAngle = Mathf.Lerp(wheel_04.steerAngle, targetSteerAngle, Time.deltaTime * turnspeed);
    }

    private void EngineSound()
{
    vol = PlayerPrefs.GetFloat("SliderValue", 0);
    pitch = currentSpeed / maxSpeed;
    transform.GetComponent<AudioSource>().pitch = pitch;
    transform.GetComponent<AudioSource>().volume = vol;
}

}