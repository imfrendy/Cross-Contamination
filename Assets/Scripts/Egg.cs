using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {
	Rigidbody body;

	public GameObject explosion;
	public GameObject player;

	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (transform.forward * 120f + transform.up * 20f);
		gameObject.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
		Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag != "Details") {
			if (collision.gameObject.tag == "Enemy") {
				// weird damage scaling but eh
				//collision.gameObject.SendMessage("ApplyDamage", Vector3.Magnitude(body.velocity));
			}

			Eggsplode();
			Destroy (gameObject);
			// add egg breaking effect
		}
	}

	void Eggsplode() {
		GameObject[] enemies;

		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			// needs rigidbody for explosion to work
			if (enemy.GetComponent<Rigidbody> () != null) {
				// add an explosion function to the enemies that takes position as argument
				enemy.SendMessage ("AddEggExplosion", transform.position);
			}
		}

		explosion.SetActive (true);
		Instantiate (explosion, transform.position, Quaternion.identity);
		explosion.SetActive (false);
	}
}