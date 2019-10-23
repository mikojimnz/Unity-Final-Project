using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

	public float speed;
	public Slider points;
	public Text distance;
	public float controlDelay;
	public float xMultiplier = 3f;
	public float yMultiplier = 3f;
	public float forwardSpeed = 0.5f;
	public float returnSpeed = 2.0f;
	public AudioSource deathSound;
	public AudioSource pointSound;

	private Rigidbody rb;
	private static float pointCount;
	private bool controlDisabled;
	private bool gameActive;
	private bool lockedUpdate;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		Cursor.visible = false;
		distance.text = "";
		pointCount = 100f;
		controlDisabled = true;
		gameActive = true;
		StartCoroutine (EnableControl ());
		InvokeRepeating ("AdjustPoint", 1f, 1f);
	}

	void FixedUpdate ()
	{
		if (pointCount <= 0) {
			GameLost ();
		}

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

		float moveHorizontal;
		float moveVertical;

		if (!gameActive) {
			Vector3 stopMovement = new Vector3 (0f, 0f, -10f);
			rb.AddForce (stopMovement * speed);
			return;
		} else if (controlDisabled) {
			moveHorizontal = 0f;
			moveVertical = 0f;
		} else {
			moveHorizontal = Input.GetAxis ("Horizontal") * xMultiplier;
			moveVertical = Input.GetAxis ("Vertical") * yMultiplier;
		}

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0f);
		rb.AddForce (movement * speed);

		Vector3 zAxisMovement = rb.velocity;
		zAxisMovement.z = forwardSpeed * speed;
		rb.velocity = zAxisMovement;

		float moveX = Mathf.Lerp (transform.position.x, 0f, Time.deltaTime * returnSpeed);
		float moveY = Mathf.Lerp (transform.position.y, 0f, Time.deltaTime * returnSpeed);
		transform.position = new Vector3 (moveX, moveY, transform.position.z);

		SetText ();
	}

	private void OnTriggerEnter (Collider other)
	{
		switch (other.tag) {
		case "Point":
			pointSound.Play ();
			Destroy (other.gameObject);
			AddPoint ();
			break;
		case "StaticObject":
			deathSound.Play ();
			GameLost ();
			break;
		}
	}

	private void GameLost ()
	{
		gameActive = false;
		StartCoroutine (ResetLevel (3f));
	}

	private void SetText ()
	{
		points.value = pointCount / 100;
		distance.text = "Distance: " + (int)(Time.timeSinceLevelLoad % 60) + "m";
	}

	public static void AddPoint ()
	{
		pointCount = 100f;
	}

	void AdjustPoint ()
	{
		pointCount -= 5f;
	}

	private IEnumerator ResetLevel (float cooldown)
	{
		yield return new WaitForSeconds (cooldown);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	private IEnumerator EnableControl ()
	{
		yield return new WaitForSeconds (controlDelay);
		controlDisabled = false;
	}
}