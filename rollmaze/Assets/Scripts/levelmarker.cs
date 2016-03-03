using UnityEngine;
using System.Collections;

public class levelmarker : MonoBehaviour {
	private UILabel _label;
	private Vector3 origin=new Vector3(0.8f,0.8f,0.8f);
	public float zoomerSpeed=500f;
	private Vector3 zoomer=new Vector3(2,2,2);
	private Color color;

	public UILabel label{
		get{return  _label;}
		set{ _label = value;}
	}

	// Use this for initialization
	void Awake () {
		label = GetComponent<UILabel> ();
		color = label.color;
	}
	
	// Update is called once per frame
	public IEnumerator onChose(){
		float para = 0f;
		Color temp = color;

		while (para<=1f) {
			float u = para * para;
			float scalepara = (1-u) * 0.8f + u * 1.8f;
			float alfa = u * 255f;
			Vector3 scale = new Vector3 (scalepara,scalepara,scalepara);
			temp.a = alfa;
			label.transform.localScale = scale;

			para += Time.deltaTime * zoomerSpeed;
			yield return null;
			
		}
	}
	public IEnumerator unChose(){
		float para = 0f;
		Color temp = color;
		while (para<=1f) {
			float u = para * para;
			float scalepara = (1-u) * 1.8f+  u * 0.8f;
			float alfa = (1 - u) * 255f;
			Vector3 scale = new Vector3 (scalepara,scalepara,scalepara);	
			temp.a = alfa;
			label.transform.localScale = scale;

			para += Time.deltaTime * zoomerSpeed;
			yield return null;

		}
	}
	public void appear(){
		TweenScale tween = TweenScale.Begin (this.gameObject, 1.5f, origin);
		tween.method = TweenScale.Method.EaseOut;
	}
}
