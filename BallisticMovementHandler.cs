using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticMovementHandler : MonoBehaviour
{
    public float Drag = 0.0f;
    public float InitialSpeed = 0.0f;
    public float InitialVertAngle = 0.0f;
    public float InitialHorzAngle = 0.0f;

    public bool MarkTrajectory = false;
    public bool StopOnZeroY = true;

    private float Grav = 9.8f;

    public Vector3 Speed = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 Acceleration = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 Direction = new Vector3(0.0f, 0.0f, 0.0f);

    public float FlightTime = 0.0f;

    private int cntr = 49;
    private GameObject newObj;

    void Start() {
        Direction.x = Mathf.Cos(InitialHorzAngle) * Mathf.Cos(InitialVertAngle);
        Direction.y = Mathf.Sin(InitialVertAngle);
        Direction.z = Mathf.Sin(InitialHorzAngle) * Mathf.Cos(InitialVertAngle);

        Speed.x = Direction.x * InitialSpeed;
        Speed.y = Direction.y * InitialSpeed;
        Speed.z = Direction.z * InitialSpeed;
    }

    void FixedUpdate() {
        if(gameObject.transform.position.y > 0.0f && StopOnZeroY) {
            FlightTime += Time.fixedDeltaTime;
        }

        Acceleration.x = -Drag * Speed.x;
        Acceleration.y =  -Drag * Speed.y - Grav;
        Acceleration.z = -Drag * Speed.z;
        Speed += Acceleration * Time.fixedDeltaTime;

        if(gameObject.transform.position.y <= 0.0f && StopOnZeroY) {
            Acceleration.x = 0.0f;
            Acceleration.y = 0.0f;
            Acceleration.z = 0.0f;
            Speed.x = 0.0f;
            Speed.y = 0.0f;
            Speed.z = 0.0f;
        }

        gameObject.transform.Translate(Speed.x * Time.fixedDeltaTime, Speed.y * Time.fixedDeltaTime, Speed.z * Time.fixedDeltaTime, Space.World);
        gameObject.transform.LookAt(gameObject.transform.position + Speed);

        if(MarkTrajectory) {
            cntr++;
            if(cntr == 50 && gameObject.transform.position.y > 0.0f) {
                cntr = 0;
                newObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newObj.transform.Translate(gameObject.transform.position);
                newObj.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
            }
        }
    }
}
