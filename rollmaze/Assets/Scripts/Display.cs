using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public static Display Instance;

    public Text puzzleNumberText;
    public Text pickupsBonusText;

    public Text timeText;
	public Slider timeSlider;
	public Text oneStarTick;
	public Text twoStarTick;
	public Text threeStarTick;

	public Text levelCompleteText;
    public Text gameOverText;

	public Text pausedText;

	public GameObject[] emptyStars;
	public GameObject[] fullStars;

    private int totalNumStars = 3;

	private float sliderStep;
	private float timeStep = 0.01f;
    
    void Awake()
    {
        Instance = this;

        levelCompleteText.CrossFadeAlpha(0f, 0f, false);
        gameOverText.CrossFadeAlpha(0f, 0f, false);
		pausedText.CrossFadeAlpha(0f, 0f, false);
    }

	void Start()
    {
        UpdatePuzzleNumberDisplay(GameManager.Instance.LevelNumber);
        UpdatePickupsDisplay(GameManager.Instance.NumPickups);
		UpdateTimeDisplay(GameManager.Instance.Time);

        foreach (GameObject star in emptyStars)
        {
            star.SetActive(false);
        }

        foreach (GameObject star in fullStars)
        {
            star.SetActive(false);
        }

		PlaceTicks();

		sliderStep = 0.01f / LevelInformation.Instance.oneStarTime;

		// update time every 100th of a second
		InvokeRepeating("GameManagerUpdateTime", 1f/*LevelFader.Instance.fadeInSpeed*/, timeStep);
	}

    public void UpdatePuzzleNumberDisplay(int puzzleNumber)
    {
        puzzleNumberText.text = string.Format("Level {0}", puzzleNumber);
    }

    public void UpdatePickupsDisplay(int numPickups)
    {
        pickupsBonusText.text = string.Format("Pickups : {0} / {1}", numPickups, LevelInformation.Instance.numPickups);
    }

	public void UpdateTimeDisplay(float time)
    {
		timeText.text = string.Format("Time: {0:0.##}", time);
		timeSlider.value += sliderStep;

        if (time <= LevelInformation.Instance.threeStarTime)
        {
            timeSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (time <= LevelInformation.Instance.twoStarTime)
        {
            timeSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else if (time <= LevelInformation.Instance.oneStarTime)
        {
            timeSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else
        {
            timeSlider.fillRect.GetComponent<Image>().color = Color.gray;
        }
    }

	public void ShowLevelCompleteText()
	{
        float bestScore = GameManager.Instance.SetScoreForPuzzle(LevelInformation.Instance.GetLevelString(), GameManager.Instance.Time);

        levelCompleteText.text = string.Format("Level Complete\n\nTime: {0:0.##} seconds\nBest: {1:0.##} seconds\n\nPress enter to continue", GameManager.Instance.Time, bestScore);

        levelCompleteText.CrossFadeAlpha(1.0f, 1.0f, false);

        int numStars;
		if (GameManager.Instance.Time <= LevelInformation.Instance.threeStarTime)
        {
            numStars = 3;
        }
		else if (GameManager.Instance.Time <= LevelInformation.Instance.twoStarTime)
        {
            numStars = 2;
        }
		else if (GameManager.Instance.Time <= LevelInformation.Instance.oneStarTime)
        {
            numStars = 1;
        }
        else
        {
            numStars = 0;
        }

        for (int i = 0; i < numStars; i++)
        {
            fullStars[i].SetActive(true);
        }

        for (int i = numStars; i < 3; i++)
        {
            emptyStars[i].SetActive(true);
        }
	}

    public void ShowGameOverText()
    {
		DirectionalLight.Instance.Dim();
        gameOverText.CrossFadeAlpha(1.0f, 1.0f, false);
    }

	public void ShowPausedText()
	{
        //DirectionalLight.Instance.Dim();
        if (pausedText != null)
            pausedText.CrossFadeAlpha(1.0f, 1.0f, true);
	}

	public void HidePausedText()
	{
		//DirectionalLight.Instance.Brighten();
        if (pausedText != null)
		    pausedText.CrossFadeAlpha(0.0f, 1.0f, true);
	}

	public void StopUpdateTime()
	{
		CancelInvoke("GameManagerUpdateTime");
	}

	private void GameManagerUpdateTime()
	{
		GameManager.Instance.Time += timeStep;
	}

	private void PlaceTicks()
	{
		float realisticWidth = (timeSlider.GetComponent<RectTransform> ().rect.width - 10f);
		float realisticStart = -1f * realisticWidth / 2;

		float threeStarRatio = LevelInformation.Instance.threeStarTime / LevelInformation.Instance.oneStarTime;
		Vector3 pos = threeStarTick.GetComponent<RectTransform>().localPosition;
		pos.x = (realisticWidth * threeStarRatio) + realisticStart;
		threeStarTick.GetComponent<RectTransform>().localPosition = pos;

		float twoStarRatio = LevelInformation.Instance.twoStarTime / LevelInformation.Instance.oneStarTime;
		pos = twoStarTick.GetComponent<RectTransform>().localPosition;
		pos.x = (realisticWidth * twoStarRatio) + realisticStart;
		twoStarTick.GetComponent<RectTransform>().localPosition = pos;
	}
}
