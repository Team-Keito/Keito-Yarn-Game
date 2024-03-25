using UnityEngine;
using AK.Wwise;

public class FanController : MonoBehaviour
{
    // Reference to the Wwise event for the fan
    public AK.Wwise.Event fanEvent;
    public AK.Wwise.Event stopFanEvent;





    public void PayFan()
    {
        fanEvent.Post(gameObject);

    }

    public void StopFan()
    {
        stopFanEvent.Post(gameObject);

    }


}

