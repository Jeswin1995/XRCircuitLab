using UnityEngine;

public class WireGenerator : MonoBehaviour {
    public GameObject segmentPrefab;  // Wire segment prefab
    public int segmentCount = 20;
    public float segmentSpacing = 0.1f;
    public float wireRadius = 0.05f;
    
    private GameObject[] segments;

    void Start() {
        GenerateWire();
    }

    void GenerateWire() {
        segments = new GameObject[segmentCount];
        Rigidbody previousRb = null;

        for (int i = 0; i < segmentCount; i++) {
            GameObject segment = Instantiate(segmentPrefab, transform.position + Vector3.up * i * segmentSpacing, Quaternion.identity, transform);
            segments[i] = segment;

            Rigidbody rb = segment.GetComponent<Rigidbody>();
            rb.mass = 0.1f;

            if (previousRb != null) {
                ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();
                joint.connectedBody = previousRb;
                SetupJoint(joint);
            }

            previousRb = rb;
        }
    }

    void SetupJoint(ConfigurableJoint joint) {
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = new Vector3(0, -segmentSpacing, 0);
        joint.anchor = Vector3.zero;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Locked;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = segmentSpacing;
        joint.linearLimit = limit;
    }
}