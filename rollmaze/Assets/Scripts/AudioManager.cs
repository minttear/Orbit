using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

	public GameObject backgroundDronePrefab;
	public GameObject ballBouncePrefab;
	public GameObject labyrinthStartPrefab;
	public GameObject labyrinthSustainPrefab;
	public GameObject labyrinthEndPrefab;
	public GameObject goalHitPrefab;
	public GameObject obstacleHitPrefab;
	public GameObject pickupCollectedPrefab;
	public GameObject teleportPrefab;
	public GameObject tutorialVoicesMissionPrefab;
	public GameObject tutorialVoicesControlsPrefab;
	public GameObject tutorialVoicesPickupsPrefab;
	public GameObject tutorialVoicesObstaclesPrefab;
	public GameObject tutorialVoicesSwitchesPrefab;
	public GameObject tutorialVoicesTeleportsPrefab;
	public GameObject tutorialVoicesStickPrefab;

    private AudioSource backgroundDrone;

    private AudioSource ballBounce;

    private AudioSource labyrinthStart;
    private AudioSource labyrinthSustain;
    private AudioSource labyrinthEnd;

    private AudioSource goalHit;

    private AudioSource obstacleHit;
    private AudioSource pickupCollected;
    private AudioSource teleport;

    private AudioSource tutorialVoicesMission;
    private AudioSource tutorialVoicesControls;
    private AudioSource tutorialVoicesPickups;
    private AudioSource tutorialVoicesObstacles;
    private AudioSource tutorialVoicesSwitches;
    private AudioSource tutorialVoicesTeleports;
    private AudioSource tutorialVoicesStick;

    void Awake()
    {
		// make sure that we don't duplicate the AudioManager when a new scene loads
		if (Instance)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			Instance = this;

			InstantiateAll();

			PlayBackgroundDrone();

			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(backgroundDrone);
			DontDestroyOnLoad(ballBounce);
			DontDestroyOnLoad(labyrinthStart);
			DontDestroyOnLoad(labyrinthSustain);
			DontDestroyOnLoad(labyrinthEnd);
			DontDestroyOnLoad(goalHit);
			DontDestroyOnLoad(obstacleHit);
			DontDestroyOnLoad(pickupCollected);
			DontDestroyOnLoad(teleport);
			DontDestroyOnLoad(tutorialVoicesMission);
			DontDestroyOnLoad(tutorialVoicesControls);
			DontDestroyOnLoad(tutorialVoicesPickups);
			DontDestroyOnLoad(tutorialVoicesObstacles);
			DontDestroyOnLoad(tutorialVoicesSwitches);
			DontDestroyOnLoad(tutorialVoicesTeleports);
			DontDestroyOnLoad(tutorialVoicesStick);
		}
    }

    public void PlayBackgroundDrone()
    {
        backgroundDrone.Play();
    }

    public void StopBackgroundDrone()
    {
        backgroundDrone.Stop();
    }

	public void PlayBallBounce()
	{
		ballBounce.Play();
	}

    public void PlayLabyrinthStart()
    {
        if (labyrinthStart.isPlaying)
            return;

        labyrinthStart.Play();
    }

    public void PlayLabyrinthSustain()
    {
        if (labyrinthSustain.isPlaying)
            return;

        PlayLabyrinthStart();
        labyrinthSustain.Play();
    }

    public void StopLabyrinthSustain()
    {
        if (!labyrinthSustain.isPlaying)
            return;

        labyrinthSustain.Stop();
        PlayLabyrinthEnd();
    }

    public void PlayLabyrinthEnd()
    {
        if (labyrinthEnd.isPlaying)
            return;

        labyrinthEnd.Play();
    }

    public void PlayGoalHit()
    {
        goalHit.Play();
    }

    public void PlayPickupCollected()
    {
        pickupCollected.Play();
    }

    public void PlayObstacleHit()
    {
        obstacleHit.Play();
    }

	public void PlayTeleport()
	{
		teleport.Play();
	}

	public void PlayTutorialMission()
	{
		tutorialVoicesMission.Play();
	}

    public void StopTutorialMission()
    {
        tutorialVoicesMission.Stop();
    }

	public void PlayTutorialControls()
	{
        tutorialVoicesControls.Play();
	}

	public void PlayTutorialPickups()
	{
        tutorialVoicesPickups.Play();
	}

	public void PlayTutorialObstacles()
	{
        tutorialVoicesObstacles.Play();
	}

	public void PlayTutorialSwitches()
	{
        tutorialVoicesSwitches.Play();
	}

	public void PlayTutorialTeleports()
	{
        tutorialVoicesTeleports.Play();
	}

	public void PlayTutorialStick()
	{
		tutorialVoicesStick.Play();
	}

    public void StopTutorials()
    {
        tutorialVoicesControls.Stop();
        tutorialVoicesObstacles.Stop();
        tutorialVoicesPickups.Stop();
        tutorialVoicesTeleports.Stop();
        tutorialVoicesStick.Stop();
    }

    private void InstantiateAll()
	{
		InstantiateAndSet(ref backgroundDrone, backgroundDronePrefab);
		InstantiateAndSet(ref labyrinthStart, labyrinthStartPrefab);
        InstantiateAndSet(ref labyrinthSustain, labyrinthSustainPrefab);
        InstantiateAndSet(ref labyrinthEnd, labyrinthEndPrefab);
        InstantiateAndSet(ref ballBounce, ballBouncePrefab);
        InstantiateAndSet(ref goalHit, goalHitPrefab);
        InstantiateAndSet(ref obstacleHit, obstacleHitPrefab);
        InstantiateAndSet(ref pickupCollected, pickupCollectedPrefab);
        InstantiateAndSet(ref teleport, teleportPrefab);
        InstantiateAndSet(ref tutorialVoicesMission, tutorialVoicesMissionPrefab);
        InstantiateAndSet(ref tutorialVoicesControls, tutorialVoicesControlsPrefab);
        InstantiateAndSet(ref tutorialVoicesObstacles, tutorialVoicesObstaclesPrefab);
        InstantiateAndSet(ref tutorialVoicesPickups, tutorialVoicesPickupsPrefab);
        InstantiateAndSet(ref tutorialVoicesSwitches, tutorialVoicesSwitchesPrefab);
        InstantiateAndSet(ref tutorialVoicesTeleports, tutorialVoicesTeleportsPrefab);
        InstantiateAndSet(ref tutorialVoicesStick, tutorialVoicesStickPrefab);
	}

	private void InstantiateAndSet(ref AudioSource sourceToSet, GameObject prefab)
	{
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        sourceToSet = go.GetComponent<AudioSource>();
	}
}
