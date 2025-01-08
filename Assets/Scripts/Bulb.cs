using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bulb : MonoBehaviour
{
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public Light pointLight;

    private Wire wire1;
    private Wire wire2;

    void Start()
    {
        if (pointLight == null) return;
        pointLight.enabled = false;
    }
    void Update()
    {
        if (!SocketsConnected())
        {
            pointLight.enabled = false;
            return;
        }
        CheckSockets();
        UpdateLightStatus();
    }

    private bool SocketsConnected()
    {
        return socket1.hasSelection && socket2.hasSelection;
    }

    private void CheckSockets()
    {
        wire1 = GetWireFromSocket(socket1);
        wire2 = GetWireFromSocket(socket2);
    }

    private Wire GetWireFromSocket(XRSocketInteractor socket)
    {
        if (socket.hasSelection)
        {
            GameObject attachedObject = socket.GetOldestInteractableSelected().transform.gameObject;
            if (attachedObject.CompareTag("Wire"))
            {
                return attachedObject.GetComponent<Wire>();
            }
        }
        return null;
    }

    private void UpdateLightStatus()
    {
        if (wire1 != null && wire2 != null)
        {
            if (wire1.isPowered && wire2.isPowered)
            {
                pointLight.enabled = true;
                return;
            }
        }
        pointLight.enabled = false;
    }
}