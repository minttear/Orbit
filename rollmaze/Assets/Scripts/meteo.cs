using UnityEngine;
using System.Collections;

public class meteo : MonoBehaviour {
	public float max;
	public float min;
	private float time;
	// Use this for initialization
	 void Start () {
		time = Random.Range (0.5f, 1f);
		TweenPosition tween = TweenPosition.Begin(this.gameObject,time,transform.position);
		float y = Random.Range (min, max);
		Vector3 end = new Vector3 (-25, y, -6);
		tween.to = end;
		Invoke ("des", time);


	}
	void des(){
		Destroy (this.gameObject);
	}
}
