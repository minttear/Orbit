using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
	public GameObject correspondingTeleport;

    public bool isEntering = false;

	void OnTriggerEnter(Collider coll)
	{
        if (isEntering)
            return;
        
		if (coll.gameObject.tag == "Ball")
		{
            TransportBall(coll.gameObject);
		}
	}

    void OnTriggerExit(Collider coll)
    {
        if (!isEntering)
            return;

        if (coll.gameObject.tag == "Ball")
        {
            isEntering = false;
        }
    }

    private void TransportBall(GameObject ball)
    {
		AudioManager.Instance.PlayTeleport();
        correspondingTeleport.GetComponent<Teleport>().isEntering = true;

        //ball.transform.position = correspondingTeleport.gameObject.transform.position;
		ball.GetComponent<Ball>().TeleportTo(gameObject, correspondingTeleport.gameObject);
    }
}
