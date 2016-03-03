using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public GameObject[] cylins;
	public GameObject[] Sticks;
	public GameObject[] switches;
	public GameObject stickTo;

    private GameObject directionalLight;

	public bool IsBallAlive { get; set; }
	public bool RotatingDisabled { get; set;}

    private int _puzzleNumberMajor; // i.e. "0" in "0.1"

	private Vector3 _mouse_last_position;

	private Vector3 _center;

	public enum RotatingState {clockwise, counterclockwise, stop};

	private RotatingState _rotating_state;

	private int _mouse_pause_counter;

	private bool isPaused = false;

	private  bool _isStick = false;

	public RotatingState Rotating_State{
		get{ return _rotating_state;}
		set{ _rotating_state = value;}
	}

    private int _levelNumber;
    public int LevelNumber
    { 
        get
        {
            return _levelNumber;
        }
        set
        {
            _levelNumber = value;
            Display.Instance.UpdatePuzzleNumberDisplay(_levelNumber);
        }
    }

    private int _numPickups;
    public int NumPickups
    {
        get
        {
            return _numPickups;
        }
        set
        {
            _numPickups = value;
            Display.Instance.UpdatePickupsDisplay(_numPickups);
        }
    }

    private float _time;
	public float Time
    {
        get
        {
			return _time;
        }
        set
        {
            if (!isPaused)
            {
                _time = value;
                Display.Instance.UpdateTimeDisplay(_time);
            }
        }
    }
	public bool isStick{
		get{ return _isStick;}
		set{ _isStick = value;
			if (value == false) {
				if (Ball.Instance == null)
					return;
				Ball.Instance.transform.SetParent (null);
				Ball.Instance.rb.constraints = RigidbodyConstraints.FreezePositionZ;
				Ball.Instance.rb.useGravity = true;
				if(stickTo!=null)
				stickTo.GetComponent<cylin> ().DecreaseSpeed ();

			}
		}
	}

    void Awake()
    {
        // make sure that we don't duplicate the GameManager when a new scene loads
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;

			ResetScores();

            DontDestroyOnLoad(gameObject);

            IsBallAlive = true;
        }
    }


    void Start()
    {
		IsBallAlive = true;
		_center = new Vector3 (267f, 184.5f, 0f);
		_mouse_last_position = Vector3.zero;
		_rotating_state = RotatingState.stop;
		_mouse_pause_counter = 20;
		RotatingDisabled = false;
		LoadSticks ();
		LoadCylinders();
		LoadSwitches();
			
    }

    void OnLevelWasLoaded(int level)
    {
        AudioManager.Instance.PlayLabyrinthEnd();
		Goal.Instance.hasBeenHit = false;
		LevelFader.Instance.FadeIn();
		RotatingDisabled = false;
        _time = 0;
        _numPickups = 0;
        IsBallAlive = true;
        LoadCylinders();
		LoadSwitches();
		LoadSticks ();
        SetPaused(false);
    }

    void Update()
	{	if (Input.GetKeyDown (KeyCode.Space) && isStick) {
			isStick = false;
			Rotating_State= RotatingState.stop;
		}
        if (Input.GetKeyUp(KeyCode.P))
        {
            SetPaused(!isPaused);
        }

        if (Input.GetKey(KeyCode.R))
        {
			AudioManager.Instance.StopTutorials();
            LoadLevel(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKey(KeyCode.M))
        {
			AudioManager.Instance.StopTutorials();
            LoadLevel("_Scene_MainMenunew");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (!isPaused)
        {
            UpdateMovement();
        }
	}

	public void LevelComplete()
	{
        DirectionalLight.Instance.Dim();

		Display.Instance.StopUpdateTime();
        Display.Instance.ShowLevelCompleteText();

		Tutorial.Instance.gameObject.SetActive(false);
	}

	public void ResetLevel()
	{
		foreach (GameObject cylin in cylins)
		{
			cylin.GetComponent<cylin>().Reset();
		}
		foreach (GameObject cylin in Sticks)
		{
			cylin.GetComponent<cylin>().Reset();
		}


		foreach (GameObject sw in switches)
		{
			sw.GetComponent<Switch>().Reset();
		}
	}

	// will return the best score for level
	public float GetBestScoreForPuzzle(string puzzleName)
	{
        if (PlayerPrefs.HasKey(puzzleName))
        {
            return PlayerPrefs.GetFloat(puzzleName);
        }
        else
        {
            return -1.0f;
        }
	}

	// if score is higher than current best score for puzzleName, will
	// set high score. will then return most recent high score
	public float SetScoreForPuzzle(string puzzleName, float score)
	{
        float currentBestScore = GetBestScoreForPuzzle(puzzleName);

        if (currentBestScore == -1.0f) // if score hasn't been set yet
        {
            currentBestScore = score;
            PlayerPrefs.SetFloat(puzzleName, score);
        }
        else // else, if new score is better than old score, set it
        {
            if (score < currentBestScore)
            {
                currentBestScore = score;
                PlayerPrefs.SetFloat(puzzleName, score);
            }
        }

        return currentBestScore;
	}

    private void UpdateMovement()
    {
        if (IsBallAlive && !RotatingDisabled) { 
            // Control the cylins with keyboard
            if (Input.GetKey (KeyCode.A)) {
                _rotating_state = RotatingState.counterclockwise;
            } 
            else if (Input.GetKey (KeyCode.D)) {
                _rotating_state = RotatingState.clockwise;
            }
            // TODO: Control the cylins with mouse
            else if (Input.GetKey (KeyCode.Mouse0)) {
                Vector3 mouse_current_position = Input.mousePosition - _center;
                if (_mouse_last_position != Vector3.zero) {
                    Vector3 cross_product = Vector3.Cross (_mouse_last_position, mouse_current_position);
                    if (cross_product.z > 0) {
                        _mouse_pause_counter = 0;
                        _rotating_state = RotatingState.counterclockwise;
                    } else if (cross_product.z < 0) {
                        _mouse_pause_counter = 0;
                        _rotating_state = RotatingState.clockwise;
                    } else {
                        if (++_mouse_pause_counter > 20) {
                            _rotating_state = RotatingState.stop;
                        }
                    }
                }
                _mouse_last_position = mouse_current_position;
            } else {
                _rotating_state = RotatingState.stop;
                _mouse_last_position = Vector3.zero;
            }
        } else {
            _rotating_state = RotatingState.stop;
        }

        switch (_rotating_state) {
		case RotatingState.clockwise:
			if (!isStick) {
				foreach (GameObject cylin in cylins) {
					cylin.GetComponent<cylin> ().IncreaseSpeed (true);
				}
				foreach (GameObject stick in Sticks) {
					stick.GetComponent<cylin> ().IncreaseSpeed (true);
				}
			} else {
				if (stickTo != null)
					stickTo.GetComponent<cylin> ().IncreaseSpeed (true);

			}
				

                AudioManager.Instance.PlayLabyrinthSustain ();
                break;
		case RotatingState.counterclockwise:
				if (!isStick) { 
					foreach (GameObject cylin in cylins) {
						cylin.GetComponent<cylin> ().IncreaseSpeed (false);
					}
					foreach (GameObject stick in Sticks) {
						stick.GetComponent<cylin> ().IncreaseSpeed (false);
					}
					
				}else {
					if (stickTo != null)
						stickTo.GetComponent<cylin> ().IncreaseSpeed (false);
				}
					

                AudioManager.Instance.PlayLabyrinthSustain ();
                break;
		case RotatingState.stop:
			if (true) {
				foreach (GameObject cylin in cylins) {
					cylin.GetComponent<cylin> ().DecreaseSpeed ();
				}
				foreach (GameObject stick in Sticks) {
					stick.GetComponent<cylin> ().DecreaseSpeed ();
				}
			} else {
				if (stickTo != null)
					stickTo.GetComponent<cylin> ().DecreaseSpeed ();
			}

				
                if (!RotatingDisabled) {
                    AudioManager.Instance.StopLabyrinthSustain ();
                }
                break;
        }

        if (Goal.Instance.hasBeenHit && Input.GetKey(KeyCode.Return))
        {
			AudioManager.Instance.StopTutorials();
            LoadLevel(LevelInformation.Instance.nextScene);
        }

        if (Input.GetKey(KeyCode.N))
        {
			AudioManager.Instance.StopTutorials();
            LoadLevel(LevelInformation.Instance.nextScene);
        }
    }

    private void OnGameOver()
    {
        Display.Instance.ShowGameOverText();
    }

    private void LoadCylinders()
    {
        cylins = GameObject.FindGameObjectsWithTag("Labyrinth");
    }

	private void LoadSwitches()
	{
		switches = GameObject.FindGameObjectsWithTag("Switch");
	}
	private void LoadSticks()
	{
		Sticks = GameObject.FindGameObjectsWithTag ("Stick");
	}
	private void ResetScores()
	{
        _numPickups = 0;
		_time = 0;
	}

    private void SetPaused(bool newPauseState)
	{
        isPaused = newPauseState;

        if (isPaused)
        {
            Display.Instance.ShowPausedText();
			UnityEngine.Time.timeScale = 0;
        }
        else
        {
            Display.Instance.HidePausedText();
			UnityEngine.Time.timeScale = 1;
        }
	}

    private void LoadLevel(string scene)
    {
		LevelFader.Instance.FadeOut();
		isStick = false;
		SceneManager.LoadScene(scene);
    }
}
