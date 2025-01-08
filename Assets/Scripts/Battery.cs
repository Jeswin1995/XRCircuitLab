using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Battery : MonoBehaviour
{
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;

    private GameObject wire1;
    private GameObject wire2;

    void OnEnable()
    {
        // Register event listeners for when a wire exits the socket
        socket1.selectExited.AddListener(OnWireExit);
        socket2.selectExited.AddListener(OnWireExit);
    }

    void OnDisable()
    {
        socket1.selectExited.RemoveListener(OnWireExit);
        socket2.selectExited.RemoveListener(OnWireExit);
    }

    void Update()
    {
        CheckSockets();
        PowerWires();
    }

    private void CheckSockets()
    {
        // Get the wire from each socket if there is one attached
        wire1 = GetWireFromSocket(socket1);
        wire2 = GetWireFromSocket(socket2);
    }

    private GameObject GetWireFromSocket(XRSocketInteractor socket)
    {
        if (socket.hasSelection)
        {
            IXRSelectInteractable interactable = socket.GetOldestInteractableSelected();
            if (interactable != null)
            {
                GameObject attachedObject = interactable.transform.gameObject;
                if (attachedObject.CompareTag("Wire"))
                {
                    return attachedObject;
                }
            }
        }
        return null;
    }

    private void PowerWires()
    {
        // Enable power for the wires that are attached to the sockets
        if (wire1 != null)
        {
            wire1.GetComponent<Wire>().UpdatePowerState(true);
        }

        if (wire2 != null)
        {
            wire2.GetComponent<Wire>().UpdatePowerState(true);
        }
    }
    
    // This method is called when a wire exits the socket
    private void OnWireExit(SelectExitEventArgs args)
    {
        // Check which socket triggered the exit and disable power for the corresponding wire
        if (args.interactorObject == socket1 && wire1 != null)
        {
            wire1.GetComponent<Wire>().UpdatePowerState(false);  // Disable power for wire1
            wire1 = null;  // Reset the reference to the wire
            Debug.Log("Wire 1 exited, power disabled.");
        }
        else if (args.interactorObject == socket2 && wire2 != null)
        {
            wire2.GetComponent<Wire>().UpdatePowerState(false);  // Disable power for wire2
            wire2 = null;  // Reset the reference to the wire
            Debug.Log("Wire 2 exited, power disabled.");
        }
    }
}
