using UnityEngine;
using System.Collections;

public class backgroundball : MonoBehaviour {
	public float width;
	public float height;
	public float speed;
	public float rotateSpeed;
	// Use this for initialization
	void Start () {
		//StartCoroutine (movement ());
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rotate = new Vector3 (0, rotateSpeed, 0);
		transform.Rotate (rotate);

	}
	private IEnumerator movement(){
		

		while(true){
			float u = Time.time;
			float k = Mathf.Sin (u);
			float theta=u*speed;
			float x=width*Mathf.Cos(theta);
			float y = (1 - k) * height / 2 - k * height / 2;
			Vector3 pos=new Vector3(x,y,0);
			transform.position=pos;

			yield return null;
		}
			


	}
}
