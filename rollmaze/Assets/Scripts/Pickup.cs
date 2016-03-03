using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            AudioManager.Instance.PlayPickupCollected();
            OnPickup();
        }
    }

    // to be overridden by other pickups which have unique behavior (e.g. rotate board)
    protected virtual void OnPickup()
    {
        Destroy(gameObject);
    }
}
