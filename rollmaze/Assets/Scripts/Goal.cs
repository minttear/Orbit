using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
	public static Goal Instance;

	public bool hasBeenHit = false;

	void Awake()
	{
		Instance = this;

        Disable();
	}

    void FixedUpdate()
    {
        if (GameManager.Instance.NumPickups >= LevelInformation.Instance.numPickups)
        {
            Goal.Instance.Enable();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            AudioManager.Instance.PlayGoalHit();
			coll.GetComponent<Ball>().OnLevelComplete();
            GameManager.Instance.LevelComplete();

			hasBeenHit = true;
        }
    }

    public void Disable()
    {
        Behaviour halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void Enable()
    {
        Behaviour halo = (Behaviour)GetComponent("Halo");
        halo.enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
}
