using UnityEngine;
using System.Collections;

public class Bread : MonoBehaviour {
    public GameObject playerController;
    public GameObject particle;

	Rigidbody body;

	public float maxHealth = 80f;
	float health;

	public float moveSpeed = 5f;

	public float maxDistance = 20f;
	public float lungeDistance = 8f;

	RaycastHit hit;

	float timeSinceRotate;
	public float timeToRotate = 3f;
	bool turning;
	public float turnDuration = 1f;
	float turnDegs;

	public LayerMask layerMask;

	public float stillDuration = 0.5f;
	public float moveDuration = 0.5f;
	float timeProgression;
	bool moving;
	bool jumping;
	bool wasGrounded;

	public float timeToJump = 0.23f;
	bool willJump;

	public float restDuration = 2f;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		body = GetComponent<Rigidbody> ();
		turning = false;

		timeProgression = 0;
		moving = false;

		body.centerOfMass = new Vector3(0, -0.5f, 0);

		jumping = false;

		// use this for jumping
		wasGrounded = true;

		willJump = false;
	}

	// Update is called once per frame
	void Update () {
        //Stops from letting explosions constantly happen
        particle.SetActive(false);
		if (transform.position.y < -10)
			Die ();

		if (Physics.Linecast (transform.position, playerController.transform.position, out hit, layerMask)) {
			//Debug.DrawLine (transform.position, playerController.transform.position);
			// if worm can see player & within range
			if (hit.collider.tag == "Player" &&
				Vector3.Distance (transform.position, playerController.transform.position) <= maxDistance) {
				//transform.LookAt (playerController.transform);
				Vector3 targetDir = playerController.transform.position - transform.position;
				float step = 2f * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);


				// here's my really convoluted solution to make it turn
				// keep old x rot
				float oldX = transform.rotation.eulerAngles.x;
				Quaternion oldRot = transform.rotation;
				transform.rotation = Quaternion.LookRotation (newDir);
				Vector3 temp = transform.eulerAngles;
				temp.x = oldX;
				transform.rotation = oldRot;

				body.MoveRotation (Quaternion.Euler(temp));

				// lunge when in range
				if (Vector3.Distance (transform.position, playerController.transform.position) <= lungeDistance &&
					IsGrounded() && !jumping && !willJump) {
					willJump = true;
					timeProgression = 0;
					BroadcastMessage ("Jump");
				}
			} else {
				if (timeSinceRotate >= timeToRotate) {
					if (timeSinceRotate - timeToRotate <= turnDuration) {
						if (!turning) {
							turnDegs = Random.Range (-180f, 180f);
							turning = true;
						}
						body.MoveRotation(body.rotation * Quaternion.Euler(new Vector3 (0, turnDegs * Time.deltaTime, 0)));
					} else {
						turning = false;
						timeSinceRotate = 0;
					}

				}
			}
		} else {
			print ("Linecast is not working");
		}

		if (jumping) {
			if (!wasGrounded && IsGrounded()) {
				jumping = false;

				// stop for a sec
				// TODO
			}
		}

		if (Vector3.Distance (transform.position, playerController.transform.position) > lungeDistance) {
			if (moving && !willJump) {
				//move when it can't lunge

				// make it NOT move up
				Vector3 moveDir = Vector3.Scale(transform.TransformDirection(Vector3.forward), new Vector3(1, 0, 1));

				if (IsGrounded ()) {
					body.MovePosition (transform.position + moveDir * moveSpeed * Time.deltaTime);
					if (timeProgression > moveDuration) {
						moving = false;
						timeProgression = 0;

						// start move
						BroadcastMessage ("Crawl");
					}
				}
			} else {
				if (timeProgression > stillDuration) {
					moving = true;
					timeProgression = 0;
				}
			}
		}

		// jump!
		if (willJump && timeProgression >= timeToJump) {
			body.AddRelativeForce (new Vector3 (0, 1300f, Random.Range(2000f,3000f)));
			jumping = true;
			willJump = false;
			timeProgression = 0;
		}

		timeProgression += Time.deltaTime;
		timeSinceRotate += Time.deltaTime;

		wasGrounded = IsGrounded ();
	}

	void ApplyDamage (float damage) {
		this.health -= damage;

		// indicate health with color
		BroadcastMessage("Indicate", health / maxHealth);

		if (health <= 0) {
			Die ();
		}
	}

	void Die ()
	{
        //die and explode
        particle.SetActive(true);
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
        particle.SetActive(false);
    }

	void AddEggExplosion(Vector3 sourcePoint)
	{
		// 10.0f = radius, 1000f = force
		body.AddExplosionForce(1000f, sourcePoint, 10.0f);

		float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance(sourcePoint, transform.position)) / 10.0f);
		ApplyDamage(damage);
	}

	void AddPineappleExplosion(Vector3 sourcePoint)
	{
		// 20.0f = radius, 1000f = force
		body.AddExplosionForce(1000f, sourcePoint, 20.0f);

		float damage = Mathf.SmoothStep(0, 100, (20.0f - Vector3.Distance(sourcePoint, transform.position)) / 20.0f);
		ApplyDamage(damage);
	}

	void Push (Vector3 forceVector)
	{
		body.AddForce(forceVector);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player") {
			playerController.SendMessage ("Damage", Vector3.Magnitude (body.velocity) * 3);
		}
	}

	bool IsGrounded()
	{
		float distToGround = 0.6f;
		return Physics.Raycast(transform.position, -1 * Vector3.up, distToGround, layerMask);
	}

}