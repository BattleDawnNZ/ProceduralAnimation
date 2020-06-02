using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour {
    public Transform target;
    public Vector3 offset;

    public float smoothTime = 0.1f;
    public float smoothMax = 3f;
    private Vector3 smoothVelocity;
    void Update () {
        transform.position = Vector3.SmoothDamp (transform.position, target.position + offset, ref smoothVelocity, smoothTime, smoothMax);
    }
}
