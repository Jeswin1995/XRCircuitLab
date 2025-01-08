using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Switch : MonoBehaviour
{
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public XRSimpleInteractable buttonInteractable;

    private Wire wire1;
    private Wire wire2;
    private Wire poweredWire;  // Tracks the wire that was powered by the button press
    private bool isSystemActive = false;

    void OnEnable()
    {
        buttonInteractable.selectEntered.AddListener(OnButtonPress);
        socket1.selectExited.AddListener(OnWireExit);
        socket2.selectExited.AddListener(OnWireExit);
    }

    void OnDisable()
    {
        buttonInteractable.selectEntered.RemoveListener(OnButtonPress);
        socket1.selectExited.RemoveListener(OnWireExit);
        socket2.selectExited.RemoveListener(OnWireExit);
    }

    void Update()
    {
        if (!SocketsConnected() && isSystemActive)
        {
            DisablePoweredWire();
        }
    }

    private bool SocketsConnected()
    {
        return socket1.hasSelection && socket2.hasSelection;
    }

    private void OnButtonPress(SelectEnterEventArgs args)
    {
        if (!SocketsConnected()) return;

        wire1 = GetWireFromSocket(socket1);
        wire2 = GetWireFromSocket(socket2);

        if (wire1 != null && wire2 != null)
        {
            if (!isSystemActive)
            {
                // Activate the system and power the unpowered wire
                if (wire1.isPowered && !wire2.isPowered)
                {
                    wire2.UpdatePowerState(true);
                    poweredWire = wire2;
                }
                else if (wire2.isPowered && !wire1.isPowered)
                {
                    wire1.UpdatePowerState(true);
                    poweredWire = wire1;
                }

                isSystemActive = true;
                Debug.Log("System Activated");
            }
            else
            {
                // Deactivate the system by disabling only the wire that was powered by the button
                DisablePoweredWire();
            }
        }
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

    private void OnWireExit(SelectExitEventArgs args)
    {
        Wire exitedWire = args.interactableObject.transform.GetComponent<Wire>();

        // If the wire that exits was the powered wire, disable its power
        if (exitedWire != null && exitedWire == poweredWire)
        {
            exitedWire.UpdatePowerState(false);
            poweredWire = null;
            isSystemActive = false;
            Debug.Log("Wire Exited, Power Disabled");
        }
    }

    private void DisablePoweredWire()
    {
        if (poweredWire != null)
        {
            poweredWire.UpdatePowerState(false);
            poweredWire = null;
            isSystemActive = false;
            Debug.Log("System Deactivated - Powered Wire Disabled");
        }
    }
}
