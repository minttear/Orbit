using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    public GameObject mainMenuDisplay;
    public GameObject missionDisplay;
	public GameObject globalControlsDisplay;

    private float waitTime = 7f;

    void Awake()
    {
        Instance = this;

        missionDisplay.SetActive(false);
		globalControlsDisplay.SetActive(false);
    }

    public void LevelSelected(string scene)
    {
        StartCoroutine(LevelSelectedHelper(scene));
    }

    private IEnumerator LevelSelectedHelper(string scene)
    {
        LevelFader.Instance.FadeOut();
        yield return new WaitForSeconds(LevelFader.Instance.fadeOutSpeed);

        mainMenuDisplay.SetActive(false);
        missionDisplay.SetActive(true);

        LevelFader.Instance.FadeIn();
        AudioManager.Instance.PlayTutorialMission();
        yield return new WaitForSeconds(LevelFader.Instance.fadeInSpeed + waitTime);

        LevelFader.Instance.FadeOut();
		yield return new WaitForSeconds(LevelFader.Instance.fadeOutSpeed);

		missionDisplay.SetActive(false);
		globalControlsDisplay.SetActive(true);

		LevelFader.Instance.FadeIn();
		yield return new WaitForSeconds(LevelFader.Instance.fadeInSpeed + waitTime);

		LevelFader.Instance.FadeOut();
		yield return new WaitForSeconds(LevelFader.Instance.fadeOutSpeed);

        SceneManager.LoadScene(scene);
    }
}
