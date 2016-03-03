using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManagement : MonoBehaviour
{
    public static MainMenuManagement Instance;
	public string[] scenes;
	public List<levelmarker> ulblist;
	private int scenenum;
	private string scenechosed;
	private int scenetotal;

    public GameObject mainMenuDisplay;
    public GameObject titleDisplay;
    public GameObject sphere;
    public GameObject missionDisplay;
    public GameObject globalControlsDisplay;

    private float waitTime = 4f;
    private GameObject currentMenuShowing;

    private bool isReady = false;

    private bool isSceneSelected = false;
    private string nextScene = string.Empty;

    void Awake()
    {
		Instance = this;

        missionDisplay.SetActive(false);
        globalControlsDisplay.SetActive(false);

        currentMenuShowing = mainMenuDisplay;
    }

	void Start()
    {
		Invoke ("ready", 3f);
	}

	public void ready()
    {
        isReady = true;
		scenetotal = scenes.Length;
		LoadObjects.Instance.num = scenetotal;
		LoadObjects.Instance.Initial ();
		scenenum = 0;

		for (int i = 1; i < scenetotal; i++)
        {
			ulblist [i].appear ();
		}

		ulblist[scenenum].StartCoroutine("onChose");

	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isReady)
                return;

            if (isSceneSelected)
            {
                NextText();
            }
            else
            {
                isSceneSelected = true;
                LevelSelected(scenes[scenenum]);
            }
        }
    }

	void FixedUpdate()
    {
		if (Input.GetKeyUp (KeyCode.RightArrow) && LoadObjects.Instance.Status == LoadObjects.status.stoping)
        {
			loopScenes (false);
			LoadObjects.Instance.StartCoroutine ("counterclockwise");
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow) && LoadObjects.Instance.Status == LoadObjects.status.stoping)
        {
			loopScenes (true);
			LoadObjects.Instance.StartCoroutine ("clockwise");
		}
	}

	void loopScenes(bool up)
    {
		ulblist [scenenum].StartCoroutine("unChose");
		if (up)
        {
			scenenum++;

			if (scenenum == scenetotal)
				scenenum = 0;

			scenechosed = scenes [scenenum];
		}
        else
        {
			scenenum--;

			if (scenenum == -1)
				scenenum = scenetotal-1;

			scenechosed = scenes[scenenum];
		}
		ulblist [scenenum].StartCoroutine ("onChose");
	}

	public void loadScene(){
		SceneManager.LoadScene (scenechosed);
	}

    public void LevelSelected(string scene)
    {
        nextScene = scene;
        NextText();
    }

    private void NextText()
    {
        if (currentMenuShowing == mainMenuDisplay)
        {
            currentMenuShowing = missionDisplay;
            StartCoroutine(FadeOut());
            mainMenuDisplay.SetActive(false);
            titleDisplay.SetActive(false);
            sphere.SetActive(false);
            missionDisplay.SetActive(true);
            AudioManager.Instance.PlayTutorialMission();
            StartCoroutine(FadeIn());
        }
        else if (currentMenuShowing == missionDisplay)
        {
            currentMenuShowing = globalControlsDisplay;
            StartCoroutine(FadeOut());
            missionDisplay.SetActive(false);
            globalControlsDisplay.SetActive(true);
            StartCoroutine(FadeIn());
        }
        else if (currentMenuShowing == globalControlsDisplay)
        {
            AudioManager.Instance.StopTutorialMission();
            isSceneSelected = false;
            SceneManager.LoadScene(nextScene);
        }
    }

    private IEnumerator FadeIn()
    {
        LevelFader.Instance.FadeIn();
        yield return new WaitForSeconds(LevelFader.Instance.fadeInSpeed);
    }

    private IEnumerator FadeOut()
    {
        LevelFader.Instance.FadeOut();
        yield return new WaitForSeconds(LevelFader.Instance.fadeOutSpeed);
    }
}
