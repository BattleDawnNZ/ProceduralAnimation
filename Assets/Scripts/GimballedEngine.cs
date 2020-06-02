using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimballedEngine : MonoBehaviour {

    public Transform graphic;
    public float rotationSpeed = 1f;
    public new ParticleSystem particleSystem;
    public float particleLifeTime = 0.25f;
    public float particleMultiplier = 10f;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;

    public Vector3 externalAcceleration;

    public Vector3 prevWorldPosition;

    public Vector3 thrustVector {
        get {
            return ((prevWorldPosition + externalAcceleration * Time.deltaTime) - graphic.position) / Time.deltaTime;
        }
    }

    public Vector3 thrustDirection {
        get {
            return thrustVector.normalized;
        }
    }

    public float thrustMagnitude {
        get {
            return thrustVector.magnitude;
        }
    }

    void Start() {
        emissionModule = particleSystem.emission;
        mainModule = particleSystem.main;
    }

    void Update() {
        if (thrustMagnitude > 0f) {
            graphic.rotation = Quaternion.RotateTowards(graphic.rotation, Quaternion.LookRotation(thrustVector, transform.forward), Time.deltaTime * rotationSpeed);
            Debug.DrawRay(graphic.position, thrustVector, Color.red, 0f);
            emissionModule.rateOverTimeMultiplier = (thrustMagnitude * particleMultiplier / particleLifeTime);
            mainModule.startSpeed = thrustMagnitude / particleLifeTime;
        } else {
            emissionModule.rateOverTimeMultiplier = 0;
            mainModule.startSpeed = 0;
        }
        prevWorldPosition = graphic.position;
    }
}
