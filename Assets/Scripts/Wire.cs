using UnityEngine;

public class Wire : MonoBehaviour
{
    public bool isPowered = false;
    public Wire connectedWireEnd;

    public void UpdatePowerState(bool power)
    {
        isPowered = power;
        UpdateConnectedWire();
    }

    private void UpdateConnectedWire()
    {
        if (connectedWireEnd != null && connectedWireEnd.isPowered != isPowered)
        {
            connectedWireEnd.UpdatePowerState(isPowered);
        }
    }

}