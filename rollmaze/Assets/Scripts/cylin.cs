using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class cylin : MonoBehaviour {
	public List<float> accelerations;
	public List<float> max_speeds;
	int current_rotation_state_ind = 0;
	float rotate_speed = 0f;

	private Quaternion initialRotation;

	void Start()
	{
		initialRotation = transform.rotation;
	}

	// Interface for increasing rotation speed.
	public void IncreaseSpeed (bool reverse) {
		if (reverse) {
			rotate_speed -= accelerations[current_rotation_state_ind];
		} else {
			rotate_speed += accelerations[current_rotation_state_ind];
		}

		if (rotate_speed > max_speeds[current_rotation_state_ind]) {
			rotate_speed = max_speeds[current_rotation_state_ind];
		} else if (rotate_speed < -max_speeds[current_rotation_state_ind]) {
			rotate_speed = -max_speeds[current_rotation_state_ind];
		}
	}

	// Interface for stopping rotation
	public void DecreaseSpeed() {
		if (Mathf.Abs (rotate_speed) < Mathf.Abs (accelerations[current_rotation_state_ind])) {
			rotate_speed = 0;
		} else if (rotate_speed > 0) {
			rotate_speed -= Mathf.Abs(accelerations[current_rotation_state_ind]);
		} else {
			rotate_speed += Mathf.Abs(accelerations[current_rotation_state_ind]);
		}
	}

	public void NextState() {
		++current_rotation_state_ind;
		current_rotation_state_ind %= accelerations.Count;
	}

	public void Reset()
	{
		transform.rotation = initialRotation;
		current_rotation_state_ind = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.Rotate(0,0,rotate_speed*Time.deltaTime);
	}

	//void OnMouseDown () {
	//	NextState ();
	//}
}
