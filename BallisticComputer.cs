using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticComputer : MonoBehaviour
{
    public float HorzAngle = 0.0f;

    public float HorzOffset = 0.0f;
    public float VertOffset = 0.0f;
    public float InitialSpeed = 0.0f;
    public float Drag = 0.0f;

    public int Iterations = 10;
    public bool Compute = false;
    public bool SlowPrediction = false;

    public float PredictedAngle = 0.0f;

    private float Epsilon = 1.0E-8f;
    private float Grav = 9.8f;
    private GameObject Proj;
    private BallisticMovementHandler ProjMovHndlr;

    void Update()
    {
        if(Compute) {
            PredictedAngle = (1.0f / 2.0f * Mathf.Asin(Grav * HorzOffset / Mathf.Pow(InitialSpeed, 2)));
            for(int i = 0; i < Iterations; i++) {
                PredictedAngle = PredictedAngle - ((SlowPrediction) ? (Epsilon) : (1)) *
                                                  ((Drag * HorzOffset * Mathf.Tan(PredictedAngle)) + ((Grav * HorzOffset) / (InitialSpeed * Mathf.Cos(PredictedAngle))) + (Grav / Drag * Mathf.Log(1.0f - (Drag * HorzOffset) / (InitialSpeed * Mathf.Cos(PredictedAngle)))) - (Drag * VertOffset)) /
                                                  (((Drag * HorzOffset) / (Mathf.Pow(Mathf.Cos(PredictedAngle), 2))) + 
                                                  ((Grav * HorzOffset * Mathf.Sin(PredictedAngle)) / (InitialSpeed * Mathf.Pow(Mathf.Cos(PredictedAngle), 2))) +
                                                  (Grav / Drag * (1.0f / (1.0f - (Drag * HorzOffset) / (InitialSpeed * Mathf.Cos(PredictedAngle)))) * (Drag * HorzOffset * Mathf.Sin(PredictedAngle)) / (InitialSpeed * Mathf.Pow(Mathf.Cos(PredictedAngle), 2))));
            }
            Proj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Proj.transform.Translate(0.0f, 1.0f, 0.0f);
            Proj.transform.localScale = new Vector3(15.0f, 15.0f, 50.0f);
            ProjMovHndlr = Proj.AddComponent<BallisticMovementHandler>();
            ProjMovHndlr.Drag = Drag;
            ProjMovHndlr.InitialSpeed = InitialSpeed;
            ProjMovHndlr.InitialVertAngle = PredictedAngle;
            ProjMovHndlr.InitialHorzAngle = HorzAngle;
            ProjMovHndlr.MarkTrajectory = true;
            Compute = false;
        }
    }
}
