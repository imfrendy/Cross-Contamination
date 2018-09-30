using UnityEngine;
using System.Collections;

public class BurgerKing : MonoBehaviour {

	public float waitDuration = 2f;
	bool jumping;
	float time;
	Rigidbody body;
	public LayerMask layerMask;
	bool wasGrounded;

	public float maxHealth = 2000f;
	float health;

	public GameObject player;

	// Use this for initialization
	void Start () {
		time = 0;
		body = GetComponent<Rigidbody> ();
		jumping = false;

		wasGrounded = IsGrounded ();
		body.centerOfMass = new Vector3 (0, .3f, 0);

		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		print (IsGrounded());
		if (!jumping) {
			time += Time.deltaTime;
			if (IsGrounded () && time >= waitDuration) {
				body.AddRelativeForce (new Vector3 (0, Random.Range (8000f, 10000f), Random.Range (5000f, 7000f)));
				time = 0;
				jumping = true;
				BroadcastMessage ("Jump");
			} else {
				RotateTowards (player.transform.position, 1f);
			}
		}
		else {
			if (!wasGrounded && IsGrounded ()) {
				jumping = false;
			}
		}

		wasGrounded = IsGrounded ();
	}

	bool IsGrounded()
	{
		float distToGround = 1f;
		return Physics.Raycast(transform.position, -1 * Vector3.up, distToGround, layerMask);
	}
	

	void RotateTowards(Vector3 position, float stepSpeed)
	{
		Vector3 targetDir = position - transform.position;
		float step = stepSpeed * Time.deltaTime;
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
		//particle.SetActive(true);
		//Instantiate(particle, transform.position, transform.rotation);
		Destroy(gameObject);
		//particle.SetActive(false);
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
			player.SendMessage ("Damage", Vector3.Magnitude (body.velocity) * 5);
		}
	}
}
