using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	enum Switch_State {waiting, rotating, coolingdown}

	public cylin target_cylin;
	public float rotating_angle = 45f;
	public float rotating_speed = 50f;

	private const float cooldown = 3f;
	private float cooldownBeginTime;
	private Switch_State switch_state;
	private float angle_passed;
	// Use this for initialization
	void Start () {
		cooldownBeginTime = 0f;
		switch_state = Switch_State.waiting;
		angle_passed = 0f;
		UpdateColor ();

	}

	// Update is called once per frame
	void FixedUpdate () {
		switch (switch_state){
		case Switch_State.waiting:
			break;
		case Switch_State.rotating:
			float angle_delta = rotating_speed * (rotating_angle / Mathf.Abs (rotating_angle)) * Time.deltaTime;
			Vector3 rotation = target_cylin.transform.eulerAngles + new Vector3 (0, 0, angle_delta);
			target_cylin.transform.eulerAngles = rotation ;
			angle_passed += angle_delta;
			AudioManager.Instance.PlayLabyrinthSustain ();
			if (Mathf.Abs(angle_passed) >= Mathf.Abs(rotating_angle)) {
				GameManager.Instance.RotatingDisabled = false;
				cooldownBeginTime = Time.time;
				switch_state = Switch_State.coolingdown;
				UpdateColor ();
				AudioManager.Instance.StopLabyrinthSustain ();
			}
			break;
		case Switch_State.coolingdown:
			if (Time.time - cooldownBeginTime > cooldown) {
				switch_state = Switch_State.waiting;
				UpdateColor ();
			}
			break;
		}
	}

	void UpdateColor() {
		Color color = Color.magenta;
		switch (switch_state){
		case Switch_State.waiting:
			color = Color.cyan;
			break;
		case Switch_State.rotating:
			color = Color.yellow;
			break;
		case Switch_State.coolingdown:
			color = Color.gray;
			break;
		}

		GetComponent<MeshRenderer>().material.color = color;
	}


	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Ball") {
			if (switch_state == Switch_State.waiting) {
				GameManager.Instance.RotatingDisabled = true;
				switch_state = Switch_State.rotating;
				angle_passed = 0f;
				UpdateColor ();
				AudioManager.Instance.PlayLabyrinthSustain ();
			}
		}
	}

	public void Reset()
	{
		GameManager.Instance.RotatingDisabled = false;
		switch_state = Switch_State.waiting;
		UpdateColor ();
	}
}
