using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireMeshGenerator : MonoBehaviour {
    private LineRenderer lineRenderer;
    
    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = transform.childCount;
    }

    void Update() {
        UpdateLine();
    }

    void UpdateLine() {
        for (int i = 0; i < transform.childCount; i++) {
            lineRenderer.SetPosition(i, transform.GetChild(i).position);
        }
    }
}