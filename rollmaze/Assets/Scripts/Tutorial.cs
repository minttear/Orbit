using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum TutorialType
{
    None,
    BasicTutorial,
    PickupTutorial,
    ObstacleTutorial,
    RotationTutorial,
    SwitchTutorial,
    TeleportTutorial,
    StickTutorial
}

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public Text overviewText;
    public Text controlsText;
    public Text pickupsText;
    public Text obstacleText;
    public Text switchText;
    public Text teleportText;
    public Text stickText;

    public TutorialType tutorialType;

    private float waitTimeSeconds = 5f;

    void Awake()
    {
        Instance = this;

        overviewText.CrossFadeAlpha(0f, 0f, false);
        controlsText.CrossFadeAlpha(0f, 0f, false);
        pickupsText.CrossFadeAlpha(0f, 0f, false);
        obstacleText.CrossFadeAlpha(0f, 0f, false);
        switchText.CrossFadeAlpha(0f, 0f, false);
        teleportText.CrossFadeAlpha(0f, 0f, false);
        stickText.CrossFadeAlpha(0f, 0f, false);
    }

	// Use this for initialization
	void Start()
    {
        if (tutorialType != TutorialType.None)
		{
			DoTutorial();
		}
	}

    IEnumerator DoBasicTutorial()
    {
        AudioManager.Instance.PlayTutorialControls();
		StartCoroutine(DisplayText(controlsText, 3f));
		yield return new WaitForSeconds(waitTimeSeconds + 1f);
    }

	IEnumerator DoObstacleTutorial()
	{
        AudioManager.Instance.PlayTutorialObstacles();
		StartCoroutine(DisplayText(obstacleText));
        yield return new WaitForSeconds(waitTimeSeconds + 1f);
	}

    IEnumerator DoPickupTutorial()
    {
        AudioManager.Instance.PlayTutorialPickups();
        StartCoroutine(DisplayText(pickupsText));
        yield return new WaitForSeconds(waitTimeSeconds + 1f);
    }

    IEnumerator DoSwitchTutorial()
    {
        AudioManager.Instance.PlayTutorialSwitches();
        StartCoroutine(DisplayText(switchText));
        yield return new WaitForSeconds(waitTimeSeconds + 1f);
    }

    IEnumerator DoTeleportTutorial()
    {
        AudioManager.Instance.PlayTutorialTeleports();
        StartCoroutine(DisplayText(teleportText));
        yield return new WaitForSeconds(waitTimeSeconds + 1f);
    }

    IEnumerator DoStickTutorial()
    {
        AudioManager.Instance.PlayTutorialStick();
        StartCoroutine(DisplayText(stickText));
        yield return new WaitForSeconds(waitTimeSeconds + 1f);
    }

	IEnumerator DisplayText(Text text, float extraWaitTime = 0f)
    {
		float waitTime = waitTimeSeconds + extraWaitTime;
        ShowText(text);
		yield return new WaitForSeconds(waitTime);
        HideText(text);
    }

    void ShowText(Text text)
    {
		DirectionalLight.Instance.Dim();
        text.CrossFadeAlpha(1.0f, 1.0f, false);
    }

    void HideText(Text text)
    {
        DirectionalLight.Instance.Brighten();
        text.CrossFadeAlpha(0.0f, 1.0f, false);
    }

	void DoTutorial()
	{
		switch (tutorialType)
		{
            case TutorialType.BasicTutorial:
                StartCoroutine(DoBasicTutorial());
                break;

            case TutorialType.PickupTutorial:
                StartCoroutine(DoPickupTutorial());
			    break;

            case TutorialType.ObstacleTutorial:
                StartCoroutine(DoObstacleTutorial());
			    break;

            case TutorialType.SwitchTutorial:
                StartCoroutine(DoSwitchTutorial());
			    break;

            case TutorialType.TeleportTutorial:
                StartCoroutine(DoTeleportTutorial());
                break;

            case TutorialType.StickTutorial:
                StartCoroutine(DoStickTutorial());
                break;

		    default:
			    break;
		}
	}
}
