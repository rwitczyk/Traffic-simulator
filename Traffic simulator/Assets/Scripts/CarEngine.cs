using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{

    // nodes ze sciezki

    public Transform path;
    public float maxSteerAngle = 40f;
    public float maxMotorTorque = 80f;
    public float currentSpeed;
    public float maxSpeed = 100;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public bool carForward = false;
    public float newSterr;


    private List<Transform> nodes;
    public int currentNode = 0;
    // Use this for initialization
    void Start()
    {
        
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        // print(relativeVector);
        // magnitude - dlugosc wektora
        // relativeVector /= relativeVector.magnitude;  

        newSterr = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        if (Math.Abs(newSterr) > 5)
        {
            wheelFL.motorTorque = wheelFR.motorTorque = 0;
            currentSpeed = 0;
            wheelFL.steerAngle = newSterr;
            wheelFR.steerAngle = newSterr;
        }
        else
        {
            newSterr = 0;
        }

    }
    private void Drive()
    {
        Debug.DrawRay((transform.position + new Vector3(2, 0, 0))  ,
        ( transform.TransformDirection(Vector3.forward) + new Vector3(0,0.051f,0))* 3f
            , Color.red);


        carForward = Physics.Raycast(transform.position + new Vector3(2, 0, 0), (transform.TransformDirection(Vector3.forward) + new Vector3(0, 0.051f, 0))
           ,3f);
        if (!carForward)
          {
            currentSpeed = Mathf.PI * 2 * wheelFL.radius * wheelFL.rpm * 60 / 1000;
            if (currentSpeed < maxSpeed)
            {
                wheelFL.motorTorque = wheelFR.motorTorque = maxMotorTorque;
            }
            else
            {
                wheelFL.motorTorque = wheelFR.motorTorque = 0;
            }
        }
        else
        {
            wheelFL.motorTorque = wheelFR.motorTorque = 0;
            currentSpeed = 0;
        }



    }
    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1.5f)
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
}
