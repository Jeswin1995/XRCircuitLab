using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class Switch : MonoBehaviour
{
    public Transform switchLever;  // Reference to the lever that rotates
    public float onAngle = -30f;   // Angle when switch is on
    public float offAngle = 0f;    // Angle when switch is off

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;  // Array of sockets

    private bool isOn = false;

    void OnTriggerEnter(Collider other)
    {
        ToggleSwitch();
    }

    private void ToggleSwitch()
    {
        isOn = !isOn;
        float targetAngle = isOn ? onAngle : offAngle;
        switchLever.localRotation = Quaternion.AngleAxis(onAngle, switchLever.right);
        UpdatePowerFlow();
    }

    private void UpdatePowerFlow()
    {
        bool anySocketPowered = false;

        // Check if any socket has a powered wire
        foreach (var socket in sockets)
        {
            if (socket.hasSelection)
            {
                Wire wire = GetSelectedWire(socket);
                if (wire != null && wire.isPowered)
                {
                    anySocketPowered = true;
                    break;
                }
            }
        }

        // Pass power to all sockets based on switch state
        foreach (var socket in sockets)
        {
            if (socket.hasSelection)
            {
                Wire wire = GetSelectedWire(socket);
                if (wire != null)
                {
                    wire.UpdatePowerState(anySocketPowered && isOn);
                }
            }
        }
    }
    private Wire GetSelectedWire(XRSocketInteractor socket)
    {
        foreach (var interactable in socket.interactablesSelected)
        {
            Wire wire = interactable.transform.GetComponent<Wire>();
            if (wire != null)
            {
                return wire;
            }
        }
        return null;
    }
}