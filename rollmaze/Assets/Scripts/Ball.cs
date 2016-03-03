using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{	
	public static Ball Instance;
	public Rigidbody rb;

	public bool isInvincible;

    private Vector3 initialPosition;


    private ParticleSystem particleSystem;

    private float respawnSeconds = 2f;

	private bool isTeleportingShrink = false;
	private bool isTeleportingGrow = false;
	private float teleportTime = 1.5f;
	private GameObject teleportToObject;
	private GameObject teleportFromObject;
	private float teleportCurrentScale;
	private float smallScale;
	private float fullSizeScale;

	void Awake(){
		Instance = this;
	}

    void Start()
	{	rb = this.GetComponent<Rigidbody>();
		
        initialPosition = transform.position;

        particleSystem = GetComponent<ParticleSystem>();

        particleSystem.Stop();

		fullSizeScale = transform.localScale.x;

        GameManager.Instance.IsBallAlive = true;

    }

    void Update()
	{
		if (isTeleportingShrink) {
			rb.constraints = RigidbodyConstraints.FreezeAll;

			teleportCurrentScale = transform.localScale.x;
			teleportCurrentScale = Mathf.MoveTowards (teleportCurrentScale, smallScale, Time.deltaTime * teleportTime);
			gameObject.transform.localScale = new Vector3 (teleportCurrentScale, teleportCurrentScale, teleportCurrentScale);

			transform.position = teleportFromObject.transform.position;

			if (teleportCurrentScale <= smallScale * 1.1f) {
				isTeleportingShrink = false;
				isTeleportingGrow = true;
			}
		} else if (isTeleportingGrow) {
			teleportCurrentScale = Mathf.MoveTowards (teleportCurrentScale, fullSizeScale, Time.deltaTime * teleportTime);
			gameObject.transform.localScale = new Vector3 (teleportCurrentScale, teleportCurrentScale, teleportCurrentScale);

			transform.position = teleportToObject.transform.position;

			if (teleportCurrentScale >= fullSizeScale * 0.9f) {
				isTeleportingGrow = false;
				rb.constraints = RigidbodyConstraints.FreezePositionZ;
			}
		}

		if (rb.velocity == Vector3.zero && GameManager.Instance.isStick == false) {
			
			rb.AddForce (0.01f * (-rb.position).normalized);
//				rb.velocity = (-rb.position).normalized * 0.01;
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Labyrinth") {
			AudioManager.Instance.PlayBallBounce();
		}

		if (coll.gameObject.tag == "Obstacle") {

			if (isInvincible)
				return;
			
			AudioManager.Instance.PlayObstacleHit();
			this.OnHitObstacle();
		}
		if (coll.gameObject.tag == "Stick") {
			OnHitStick (coll.gameObject);
		}
	}
	public void OnLevelComplete()
	{
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}

    public void OnHitObstacle()
    {
        GameManager.Instance.IsBallAlive = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		isInvincible = true;
		Color boom=this.GetComponent<MeshRenderer>().materials[0].color;
		boom.a = 0;
		this.GetComponent<MeshRenderer> ().materials [0].color = boom;
        StartCoroutine(Sparkle());
        Invoke("Respawn", respawnSeconds);
    }


	private void OnHitStick(GameObject go){
		if(GameManager.Instance.stickTo!=null)
			GameManager.Instance.stickTo.GetComponent<cylin> ().DecreaseSpeed ();
		GameManager.Instance.Rotating_State = GameManager.RotatingState.stop;


		this.transform.SetParent (go.transform);

		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;

		GameManager.Instance.isStick = true;

		GameManager.Instance.stickTo = go;
		}

	public void TeleportTo(GameObject teleportFrom, GameObject teleportTo)
	{
		teleportFromObject = teleportFrom;
		teleportToObject = teleportTo;
		isTeleportingShrink = true;
	}

    private IEnumerator Sparkle()
    {
        StartCoroutine(HideBall());
		particleSystem.Clear();
		particleSystem.Simulate(particleSystem.duration);
		particleSystem.Play();
        yield return new WaitForSeconds(respawnSeconds);
		particleSystem.Stop();
    }

    private IEnumerator HideBall()
    {
        yield return new WaitForSeconds(respawnSeconds - 1f);
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Respawn()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GameManager.Instance.IsBallAlive = true;
        transform.position = initialPosition;
		this.transform.SetParent (null);

		Color boom = this.GetComponent<MeshRenderer> ().materials [0].color;
		boom.a = 255;
		this.GetComponent<MeshRenderer> ().materials [0].color = boom;

		GameManager.Instance.ResetLevel();

		rb.constraints = RigidbodyConstraints.FreezePositionZ;
		rb.useGravity = true;
		isInvincible = false;
    }
}
