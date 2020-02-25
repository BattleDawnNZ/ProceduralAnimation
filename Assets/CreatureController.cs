using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour {
    public float moveInputFactor = 5f;
    public Vector3 velocity;
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotateInputFactor = 10f;
    public float rotationSpeed = 10f;
    public float averageRotationRadius = 3f;
    private float rSpeed = 0;

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
        velocity = Vector3.MoveTowards (velocity, new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical")).normalized, Time.deltaTime * moveInputFactor);
        rSpeed = Mathf.MoveTowards (rSpeed, Input.GetAxis ("Turn") * rotationSpeed, rotateInputFactor * Time.deltaTime);
        transform.Rotate (0f, rSpeed * Time.deltaTime, 0f);
        transform.position += velocity * mSpeed * Time.deltaTime;

        if (dynamicGait) {
            timeBetweenSteps = maxTargetDistance / Mathf.Max (mSpeed * velocity.magnitude, Mathf.Abs (rSpeed * Mathf.Deg2Rad * averageRotationRadius));
        }

        if (Time.time > lastStep + (timeBetweenSteps / legs.Length) && legs != null) {
            if (legs[index] == null) return;

            Vector3 legPoint = (legs[index].restingPosition + velocity);
            Vector3 legDirection = legPoint - transform.position;
            //Vector3 rotationalPoint = (legs[index].transform.TransformDirection (Vector3.right)) * (rSpeed * Mathf.Deg2Rad * (legPoint - transform.position).magnitude);
            Vector3 rotationalPoint = ((Quaternion.Euler (0, rSpeed / 2f, 0) * legDirection) + transform.position) - legPoint;
            Debug.DrawRay (legPoint, rotationalPoint, Color.black, 1f);
            Vector3 rVelocity = rotationalPoint + velocity;

            legs[index].stepDuration = Mathf.Min (0.5f, timeBetweenSteps / 2f);
            legs[index].worldVelocity = rVelocity;
            legs[index].Step ();
            lastStep = Time.time;
            index = (index + 1) % legs.Length;
        }
    }

    public void OnDrawGizmosSelected () {
        Gizmos.DrawWireSphere (transform.position, averageRotationRadius);
    }
}
