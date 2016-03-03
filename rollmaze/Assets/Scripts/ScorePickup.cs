using UnityEngine;
using System.Collections;

public class ScorePickup : Pickup
{
	public int scoreWhenPickedUp = 1;

	protected override void OnPickup()
	{
        GameManager.Instance.NumPickups += scoreWhenPickedUp;
		base.OnPickup();
	}
}
