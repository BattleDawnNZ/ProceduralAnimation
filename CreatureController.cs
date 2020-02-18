using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour {
    public float inputFactor = 5f;
    public Vector3 velocity;
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 10f;

    public ProceduralLegPlacement[] legs;
    private int index;
    public bool dynamicGait = false;
    public float timeBetweenSteps = 0.25f;
    [Tooltip ("Used if dynamicGait is true to calculate timeBetweenSteps")] public float maxTargetDistance = 1f;
    public float lastStep = 0;

    void Start () {

    }

    void Update () {
        float mSpeed = (Input.GetButton ("Fire3") ? sprintSpeed : walkSpeed);
        velocity = Vector3.MoveTowards (velocity, new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical")).normalized, Time.deltaTime * inputFactor);
        float rSpeed = Input.GetAxis ("Mouse X") * rotationSpeed;
        transform.Rotate (0f, rSpeed, 0f);
        transform.position += velocity * mSpeed * Time.deltaTime;

        if (dynamicGait) {
            timeBetweenSteps = maxTargetDistance / Mathf.Max (mSpeed * velocity.magnitude, Mathf.Abs (rSpeed));
        }

        if (Time.time > lastStep + (timeBetweenSteps / legs.Length) && legs != null) {
            if (legs[index] == null) return;
            legs[index].stepDuration = Mathf.Min (0.5f, timeBetweenSteps / 2f);
            legs[index].worldVelocity = velocity;
            legs[index].Step ();
            lastStep = Time.time;
            index = (index + 1) % legs.Length;
        }
    }
}
