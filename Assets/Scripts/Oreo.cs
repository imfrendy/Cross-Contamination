using UnityEngine;
using System.Collections;

public class Oreo : MonoBehaviour {
	public GameObject playerController;

	Rigidbody body;

	public float maxHealth = 200f;
	float health;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();

		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10)
			Die ();
		
		body.AddRelativeTorque (new Vector3 (2500f, 0, 0) * Time.deltaTime);

		Quaternion oldR = transform.localRotation;
		transform.LookAt (playerController.transform);
		Quaternion newR = transform.localRotation;
		transform.localRotation = Quaternion.Lerp (oldR, newR, 2f * Time.deltaTime);

		// here's my really convoluted solution to make it turn
		/*
		Vector3 targetDir = playerController.transform.position - transform.position;
		float step = 4f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);

		// keep old z rot
		float oldZ = transform.rotation.eulerAngles.z;
		Quaternion oldRot = transform.rotation;
		transform.rotation = Quaternion.LookRotation (newDir);
		Vector3 temp = transform.eulerAngles;
		temp.z = oldZ;
		transform.rotation = oldRot;

		body.MoveRotation (Quaternion.Euler(temp));*/

		//print (body.velocity.magnitude);
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
			playerController.SendMessage ("Damage", Vector3.Magnitude (body.velocity) * 10);
		}
	}
}
