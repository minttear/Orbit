using UnityEngine;
using System.Collections;

public class LevelFader : MonoBehaviour
{
	public static LevelFader Instance;

	public Texture2D fadeOutTexture;
	public float fadeOutSpeed = 0.15f;
	public float fadeInSpeed = 0.5f;

	private float fadeSpeed = 0.15f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void Awake()
	{
		Instance = this;
	}

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public void FadeIn()
	{
		fadeDir = -1;
		fadeSpeed = fadeInSpeed;
	}

	public void FadeOut()
	{
		fadeDir = 1;
		fadeSpeed = fadeOutSpeed;
	}
}
