using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TimeController : MonoBehaviour {
    public KeyCode timeKey = KeyCode.Space;

    public float pressedSpeed = 0.1f;
    public float pressedFOV = 90f;
    public float releasedSpeed = 1f;
    public float releasedFOV = 60f;
    public float changeSpeed = 5f;

    public CinemachineFreeLook rig;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = releasedSpeed;
        rig.m_Lens.FieldOfView = releasedFOV;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(timeKey)) {
            Time.timeScale = Mathf.Lerp(Time.timeScale, pressedSpeed, changeSpeed * Time.deltaTime);
            rig.m_Lens.FieldOfView = Mathf.Lerp(rig.m_Lens.FieldOfView, pressedFOV, changeSpeed * Time.deltaTime);
        } else {
            Time.timeScale = Mathf.Lerp(Time.timeScale, releasedSpeed, changeSpeed * Time.deltaTime);
            rig.m_Lens.FieldOfView = Mathf.Lerp(rig.m_Lens.FieldOfView, releasedFOV, changeSpeed * Time.deltaTime);
        }
    }
}
