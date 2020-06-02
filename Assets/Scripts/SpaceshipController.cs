using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour {

    public float moveSpeed;
    public float lookSpeed;

    void Start() {

    }

    void Update() {
        Vector3 moveInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1f) * moveSpeed * Time.deltaTime;
        Vector3 lookInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), -Input.GetAxis("Turn")), 1f) * lookSpeed * Time.deltaTime;
        transform.Translate(moveInput, Space.Self);
        transform.Rotate(lookInput, Space.Self);
    }
}
