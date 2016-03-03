using UnityEngine;
using System.Collections;

public class LoadObjects : MonoBehaviour {
	public static LoadObjects Instance;
	public GameObject pref;
	public int radius;
	public float rotateSpeed; 
	public float scale;
	public enum status{rotating,stoping}
	private status _status;
	private int _num;


	public status Status{
		get{ return _status;}
		set{ _status = value;}
	}
	public int num{
		get{ return _num;}
		set{_num=value;}
	}
	// Use this for initialization
	void Awake(){
		Instance = this;
		Status = status.stoping;
	}
	
	// Update is called once per frame
	

	#region localmethods
	public IEnumerator clockwise(){
		Status = status.rotating;
		float init = transform.rotation.eulerAngles.z;
		float unit = 360 / num;
		float u = 0;
		while (u <unit) {
			Vector3 rotate = new Vector3 (0, 0, Time.deltaTime * rotateSpeed);
			this.transform.Rotate (rotate);
			u += Time.deltaTime * rotateSpeed;
			yield return null;
		}
		float delta = init+unit-transform.rotation.eulerAngles.z;
		Vector3 fix=new Vector3(0,0,delta);
		this.transform.Rotate(fix);
		Status = status.stoping;



	}
	public IEnumerator counterclockwise(){
		Status = status.rotating;
		float init = transform.rotation.eulerAngles.z;
		float unit = 360 / num;
		float u = 0;
		while (u <unit) {
			Vector3 rotate = new Vector3 (0, 0, -1*Time.deltaTime * rotateSpeed);
			this.transform.Rotate (rotate);
			u += Time.deltaTime * rotateSpeed;
			yield return null;
		}
		float delta = init-unit-transform.rotation.eulerAngles.z;
		Vector3 fix=new Vector3(0,0,delta);
		this.transform.Rotate(fix);
		Status = status.stoping;
	}
	public void Initial(){
		for (int k = 0; k < num; k++) {
			float theta = k * 2 * Mathf.PI / num;
			float x = Mathf.Sin (theta)*radius;
			float y = Mathf.Cos (theta)*radius;
			Vector3 scale3 = new Vector3 (scale, scale, scale);
			Vector3 pos = new Vector3 (x, y, 0f);
			Quaternion rot = Quaternion.Euler (0, 0,-1*k*360/num);				
			GameObject go = Instantiate (pref);
			levelmarker lbl = go.GetComponent<levelmarker>();
			UILabel ulabel = go.GetComponent<UILabel> ();
			MainMenuManagement.Instance.ulblist.Add (lbl);
			go.name = k.ToString ();
		
			go.transform.parent = this.transform;
			go.name = k.ToString ();
			go.transform.localScale = scale3;
			go.transform.localPosition = pos;
			go.transform.rotation = rot;
			ulabel.text = "Level " + (k + 1).ToString ();

		}
	}
	#endregion
}