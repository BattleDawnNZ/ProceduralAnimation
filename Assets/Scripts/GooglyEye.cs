using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglyEye : MonoBehaviour {
    public float changeChance = 0.1f;
    public Vector3 gaze;
    // Start is called before the first frame update
    void Start() {
        UpdateGaze();
    }

    // Update is called once per frame
    void Update() {
        float p = Random.Range(0f, 1f);
        if (p < changeChance) {
            UpdateGaze();
        }
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(gaze), Time.deltaTime);
    }

    public void UpdateGaze() {
        gaze = Random.onUnitSphere;
        gaze.z = Mathf.Abs(gaze.z * 2f);
    }
}
