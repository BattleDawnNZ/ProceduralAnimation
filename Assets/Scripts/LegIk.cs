using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegIk : MonoBehaviour {

    public Transform ikTarget;

    public enum IkType {
        Iterative,
        Continous
    }

    [Header ("IK Data")]
    public bool isMounted = false;
    public IkType ikType;
    [Tooltip ("Only Used If IkType is Iterative")] public int iterations = 4;

    [Header ("Segment Data")]
    public int segmentCount = 4;
    public float segmentLength = 1f;
    public float minThickness = 0.01f;
    public float maxThickness = 0.1f;

    public Mesh segmentMesh;
    public Material segmentMaterial;

    public Segment[] segments;

    void Start () {
        segments = new Segment[segmentCount];
        for (int i = 0; i < segmentCount; i++) {
            GameObject go = new GameObject ("Segment " + i);
            Transform t = go.transform;
            MeshRenderer mr = go.AddComponent<MeshRenderer> ();
            MeshFilter mf = go.AddComponent<MeshFilter> ();

            float st = Mathf.Lerp (minThickness, maxThickness, i / (float) segmentCount);
            t.localScale = new Vector3 (st, st, segmentLength);
            mr.material = segmentMaterial;
            mf.mesh = segmentMesh;

            Segment s = new Segment (t, segmentLength);
            segments[i] = s;
        }
    }

    // Update is called once per frame
    void Update () {
        switch (ikType) {
            case IkType.Continous:
                ContinousIK ();
                break;
            case IkType.Iterative:
                IterativeIK ();
                break;
            default:
                Debug.LogError ("IKType Behaviour Not Defined");
                break;
        }
    }

    public void IterativeIK () {
        for (int s = 0; s < segmentCount; s++) {
            segments[s].position = Vector3.forward * s * segmentLength;
        }
        for (int i = 0; i < iterations; i++) {
            segments[segmentCount - 1].AdjustTo (ikTarget.position);
            for (int s = segmentCount - 2; s >= 0; s--) {
                segments[s].AdjustTo (segments[s + 1].tail);
            }
            if (isMounted) Remount ();
        }
    }

    public void ContinousIK () {
        segments[segmentCount - 1].AdjustTo (ikTarget.position);
        for (int s = segmentCount - 2; s >= 0; s--) {
            segments[s].AdjustTo (segments[s + 1].tail);
        }
        if (isMounted) Remount ();
    }

    public void Remount () {
        segments[0].tail = transform.position;
        for (int i = 1; i < segmentCount; i++) {
            segments[i].tail = segments[i - 1].head;
        }
    }

    [System.Serializable]
    public class Segment {
        public Transform transform;
        public Vector3 position {
            get {
                return transform.position;
            }
            set {
                transform.position = value;
            }
        }
        public Vector3 head {
            get {
                return transform.position + transform.forward * halfLength;
            }
            set {
                transform.position = value - transform.forward * halfLength;
            }
        }
        public Vector3 tail {
            get {
                return transform.position - transform.forward * halfLength;
            }
            set {
                transform.position = value + transform.forward * halfLength;
            }
        }
        public float halfLength {
            get {
                return length * 0.5f;
            }
            set {
                length = value * 2f;
            }
        }

        float length;

        public Segment (Transform t, float l) {
            transform = t;
            length = l;
        }

        public void AdjustTo (Vector3 p) {
            LookAt (p);
            MoveTo (p);
        }

        public void LookAt (Vector3 p) {
            transform.forward = p - position;
        }

        public void MoveTo (Vector3 p) {
            head = p;
        }
    }
}
