using UnityEngine;
using System.Collections;

public class LevelInformation : MonoBehaviour
{
	public static LevelInformation Instance;

    public int levelNumber;

	public float oneStarTime;
	public float twoStarTime;
	public float threeStarTime;

    public float numPickups;

	public string nextScene;

	void Awake()
	{
		Instance = this;
	}

    void Start()
    {
        GameManager.Instance.LevelNumber = levelNumber;
    }

    public string GetLevelString()
    {
        return levelNumber.ToString();
    }
}
