using UnityEngine;
using System.Collections;

public class meteogenerate : MonoBehaviour {
	public float max;
	public float min;
	public GameObject meteo;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("generate", 2f, 3f);
	}
	
	void generate(){
		int times = Random.Range (1, 4);
		for(int i=0;i<times;i++){
			GameObject go = Instantiate<GameObject> (meteo);
			float y = Random.Range (min, max);
			Vector3 pos = new Vector3 (Random.Range(20,50), y, -6);
			go.transform.position = pos;
		}


	}
}
