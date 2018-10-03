using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAI : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45f;
    public float turnspeed = 40f;
    public WheelCollider wheel_01; //przednie kola
    public WheelCollider wheel_04;
    public WheelCollider wheel_02;
    public WheelCollider wheel_03;
    private List<Transform> nodes;
    private float targetSteerAngle = 0;
    private int currentNode = 0;
    public float maxMotorTorque = 140f;
    public float MotorTorque = 0;
    public float currentSpeed;
    public float maxSpeed = 20f;
    public bool isBraking = false;
    public Vector3 centerOfMass;
    public float maxBrakeTorque = 100000f;
    public float Brake = 100f;
    public float WaitTime = 0.5f;
    public float pitch;
    public float vol;
    [Header("Sensors")] //czujniki wykrywajace kolizje
    public float sensorLength = 6f;
    public float sensorLengthAngle = 5f;
    public Vector3 fronsSensPos = new Vector3(0,0.2f,1.2f);
    public float frontSideSensorPos = 0.5f;
    public float frontSensorAngle = 40f;
    private bool avoiding = false;
    // Use this for initialization
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        WayPoint();
        Braking();
        LerpToSteerAngle();
        EngineSound();
    }
    private void ApplySteer()
    {
        if (avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position); //wektor od samochodu do punktu ktorego ma sie przemiescic
        //print(realativeVector);
        Vector3 tmp;
        if (currentNode == nodes.Count - 2)
        {
            tmp = transform.InverseTransformPoint(nodes[0].position);

        }
        else if (currentNode == nodes.Count - 1)
        {
            tmp = transform.InverseTransformPoint(nodes[1].position);

        }

        else
        {
            tmp = transform.InverseTransformPoint(nodes[currentNode + 2].position);

        }
        tmp /= tmp.magnitude;

        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude)*maxSteerAngle;
        targetSteerAngle = newSteer;
        //Debug.Log(targetSteerAngle);
        float angle = Vector3.Angle(relativeVector, tmp);
        //Debug.Log(relativeVector.x +"aktual");
       // Debug.Log(tmp.x + "tmp");
        //if (tmp.x*-1 > 0.6f || tmp.x > 0.6f)
        if(angle>40f)
        {
           // isBraking = false;
            //isBraking = true;
            //Braking2(WaitTime);
            //isBraking = true;
            //maxMotorTorque += 100;
            // isBraking = true;
            //Braking();
        }
        //Debug.Log(wheel_01.motorTorque);

    }
    private void Drive()
    {
        //MotorTorque = maxMotorTorque;
        currentSpeed = 2 * Mathf.PI * wheel_01.radius * wheel_01.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed && wheel_03.brakeTorque==0)
        {
            wheel_01.motorTorque = maxMotorTorque;
            wheel_04.motorTorque = maxMotorTorque;
        }else
        {
            wheel_01.motorTorque = 0;
            wheel_04.motorTorque = 0;
        }

    }
    private void WayPoint() //sprawdza odleglosc miedzy nodami i zmienia jezeli jest blisko
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 7f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
    private void Braking()
    {
        //isBraking = true;
        if (isBraking)
        {

            StartCoroutine("Braking2", WaitTime);
            //isBraking = false;

        }

        //else
        //{
        wheel_02.brakeTorque = 0;
        wheel_03.brakeTorque = 0;
        // }
        isBraking = false;
    }
    private void Sensors() //raycast sensory sprawdzaja czy samochod jest na kursie kolizyjnym
    {
        RaycastHit hit;
        Vector3 sensorStartingPosition = transform.position;
        sensorStartingPosition += transform.forward * fronsSensPos.z;
        sensorStartingPosition += transform.up * fronsSensPos.y;
        float avoidmultiplayer = 0;
        avoiding = false;

       
        //sensory boczne - prawy
        sensorStartingPosition += transform.right*frontSideSensorPos;
        if (Physics.Raycast(sensorStartingPosition, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartingPosition, hit.point);
                avoiding = true;
                avoidmultiplayer -= 1f;
            }
        }

        //sensory boczne - prawy skosny
 
        else if (Physics.Raycast(sensorStartingPosition, Quaternion.AngleAxis(frontSensorAngle,transform.up)*transform.forward, out hit, sensorLengthAngle))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartingPosition, hit.point);
                avoiding = true;
                avoidmultiplayer -= 1f;
            }
        }



        //sensory boczne - lewy - dwa razy odejmujemy zeby przejsc na lewy bok
        sensorStartingPosition -= transform.right * frontSideSensorPos*2;
        if (Physics.Raycast(sensorStartingPosition, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartingPosition, hit.point);
                avoiding = true;
                avoidmultiplayer += 1f;
            }
        }


        //sensory boczne - lewy skosny

        else if (Physics.Raycast(sensorStartingPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLengthAngle))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartingPosition, hit.point);
                avoiding = true;
                avoidmultiplayer += 1f;
            }
        }

        //sensor przedni
        if (avoidmultiplayer == 0)
        {
            if (Physics.Raycast(sensorStartingPosition, transform.forward, out hit, sensorLength)) //sprawdzamy prosta normalna obiektuy zeby wiedziec w ktora strone skrecic
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartingPosition, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidmultiplayer = -1;
                    }
                    else
                        avoidmultiplayer = 1;
                }
            }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidmultiplayer;
        }

    }
    private void LerpToSteerAngle()
    {
        wheel_01.steerAngle = Mathf.Lerp(wheel_01.steerAngle, targetSteerAngle, Time.deltaTime*turnspeed);
        wheel_04.steerAngle = Mathf.Lerp(wheel_04.steerAngle, targetSteerAngle, Time.deltaTime * turnspeed);
    }
    IEnumerator Braking2(float Count)
    {
        //wheel_02.brakeTorque = maxBrakeTorque;
        //wheel_03.brakeTorque = maxBrakeTorque;
        //Debug.Log(wheel_02.brakeTorque);
        //MotorTorque = maxMotorTorque - 100;
        wheel_02.brakeTorque = maxBrakeTorque;
        wheel_03.brakeTorque = maxBrakeTorque;
        yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
                                                //And here goes your method of resetting the game...

        wheel_02.brakeTorque = maxBrakeTorque;
        wheel_03.brakeTorque = maxBrakeTorque;
       // isBraking = true;
        //isBraking = true;
        yield return null;
    }
    private void EngineSound()
    {
        vol = PlayerPrefs.GetFloat("SliderValue", 0);
        pitch = currentSpeed / maxSpeed;
        transform.GetComponent<AudioSource>().pitch = pitch;
        transform.GetComponent<AudioSource>().volume = vol;
    }

}