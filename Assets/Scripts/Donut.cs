using UnityEngine;
using System.Collections;

public class Donut : MonoBehaviour {
	public GameObject playerController;
    public GameObject particle;

	public float maxHealth = 60f;
	float health;

	public float RotationSpeed = 10f;

	// damage when within 10m
	public float bombDist = 10f;

	//public float maxDPS = 10f;

	LineRenderer line;
	Vector3 direction;

	public float timeToRotate = 3f;
	float rotateTimer;

	public GameObject Sprinkle;
	public float rateOfFire = .1f;
	float bombTimer;

	public float boundsRange = 150f;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		//transform.LookAt (playerController.transform);

		line = GetComponent<LineRenderer> ();
		rotateTimer = 0f;
		direction = new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f)).normalized;

		bombTimer = 0f;
	}

	// Update is called once per frame
	void Update () {
        //makes it so explosions don't happen 24/7
        particle.SetActive(false);

        if (transform.position.y < 0)
			Die ();

		//gameObject.GetComponent<Rigidbody> ().AddForce (transform.forward);
		//transform.Rotate(new Vector3(0, 0, 360f * Time.deltaTime));

		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();

		// when I am within range of the player: move towards
		// when I am out of range, pick arbitrary direction every few seconds

		if (Vector3.Distance (transform.position, playerController.transform.position) <= 75) {
			// aim for directly overhead the player
			direction = ((playerController.transform.position + new Vector3(0, 4, 0)) - transform.position).normalized;
			rotateTimer = 0;
		} else {
			rotateTimer += Time.deltaTime;

			if (rotateTimer >= timeToRotate) {
				rotateTimer = 0;
				direction = new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f)).normalized;

				// todo: point towards just above origin when out of bounds
				if (Vector3.Distance (transform.position, new Vector3 ()) > boundsRange) {
					// point to center
					direction = (new Vector3 () - transform.position).normalized;
				}
			}
		}
		if (Vector3.Distance (transform.position, playerController.transform.position) >= 100) {
			rb.AddForce (200 * direction * Time.deltaTime);
		} else {
			// not so fast as it approaches player
			rb.AddForce ((200 - (2 * Vector3.Distance (transform.position, playerController.transform.position)))
				* direction * Time.deltaTime);
		}

		//Quaternion lookRotation = Quaternion.LookRotation (direction);

		//transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

		// constant rotation along the local y axis
		//rb.MoveRotation (rb.rotation * Quaternion.Euler (transform.TransformDirection(new Vector3 (0, RotationSpeed * Time.deltaTime, 0))));

		rb.MoveRotation(Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0) * 
			Quaternion.Euler (transform.TransformDirection(new Vector3 (0, RotationSpeed * Time.deltaTime, 0))));

		// bomb when 1m overhead and within range
		if (Vector3.Distance (transform.position, playerController.transform.position) < bombDist 
			&& transform.position.y >= playerController.transform.position.y + 1f) {
			// add bombs/sec
			if (bombTimer >= rateOfFire) {
				DropSprinkle ();
				bombTimer = 0;
			}

			bombTimer += Time.deltaTime;

			// TODO: add a lightning particle effect or something to show damage
			//AOEDamagePlayer (playerController, AOEdist, maxDPS * Time.deltaTime);

			//line.enabled = true;
			//line.SetPositions (new Vector3[] { transform.position, playerController.transform.position });
		} else {
			//line.enabled = false;
		}


	}

	void DropSprinkle ()
	{
		Vector3 startPt = transform.position
		                  + transform.up * -1.5f;

		Sprinkle.SetActive (true);
		Instantiate (Sprinkle, startPt, Random.rotation);
		Sprinkle.SetActive (false);
	}

	void ApplyDamage (float damage)	{
		this.health -= damage;

		// indicate health with color
		BroadcastMessage("Indicate", health / maxHealth);

		if (health <= 0) {
			Die ();
		}
	}

	void Die ()
	{
        // die and explode
        particle.SetActive(true);
        Instantiate(particle,transform.position,transform.rotation);
        Destroy (gameObject);
        particle.SetActive(false);
    }

	// currently the cube will explode away if it touches the player
	void AOEDamagePlayer(GameObject player, float maxDist, float maxDmg)
	{
		float dist = Vector3.Distance (transform.position, player.transform.position);

		// it should be, but this is just a safeguard
		if (dist < maxDist) {
			// linear scaling
			player.SendMessage ("Damage", ((maxDist - dist) / maxDist) * maxDmg);
		}
	}

	// the issue is that these explosion functions (rigidbody based) are missing on its children
	// which are also considered "enemies" to appease the raycast (mesh collider based)

	// SOLUTION: only applies method to enemies with rigidbodies
	void AddEggExplosion (Vector3 sourcePoint)
	{
		// 40.0f = radius, 5000f = force
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddExplosionForce(1000f, sourcePoint, 10.0f);

		float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance (sourcePoint, transform.position) ) / 10.0f);
		ApplyDamage (damage);
	}

	void AddPineappleExplosion (Vector3 sourcePoint)
	{
		// 40.0f = radius, 5000f = force
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddExplosionForce (1000f, sourcePoint, 20.0f);

		float damage = Mathf.SmoothStep(0, 100, (20.0f - Vector3.Distance (sourcePoint, transform.position)) / 20.0f);
		ApplyDamage (damage);
	}

	void Push (Vector3 forceVector)
	{
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce (forceVector);
	}
}
