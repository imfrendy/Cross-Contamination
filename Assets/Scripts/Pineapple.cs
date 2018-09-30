using UnityEngine;
using System.Collections;

public class Pineapple : MonoBehaviour {
	Rigidbody body;

	public GameObject explosion;

	// Use this for initialization
	public float lifespan = 30f;
	float elapsedTime;

	void Start () {
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (transform.forward * 2000f);
		transform.localRotation *= Quaternion.Euler (90f, 0, 0);
		elapsedTime = 0f;
	}

	// Update is called once per frame
	void Update () {
		if (elapsedTime >= lifespan) {
			Destroy (gameObject);
		}

		elapsedTime += Time.deltaTime;
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag != "Details") {
			if (collision.gameObject.tag == "Enemy") {
				// weird damage scaling but eh
				//collision.gameObject.SendMessage("ApplyDamage", Vector3.Magnitude(body.velocity));
			}

			Explode();
			Destroy (gameObject);
			// add pineapple explosion effect
		}
	}

	void Explode() {
		GameObject[] enemies;

		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		// needs rigidbody for explosion to work
		foreach (GameObject enemy in enemies) {
			if (enemy.GetComponent<Rigidbody> () != null) {
				// add an explosion function to the enemies that takes position as argument
				enemy.SendMessage ("AddPineappleExplosion", transform.position);
			}
		}
		explosion.SetActive (true);
		Instantiate (explosion, transform.position, Quaternion.identity);
		explosion.SetActive (false);
	}
}
