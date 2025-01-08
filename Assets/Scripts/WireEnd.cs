using UnityEngine;

public class WireEnd : MonoBehaviour
{
    public bool isPowered = false;

    public void SetPowerState(bool power)
    {
        isPowered = power;
    }

}