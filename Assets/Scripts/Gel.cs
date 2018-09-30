using UnityEngine;
using System.Collections;

public class Gel : MonoBehaviour {
	Rigidbody body;

	public GameObject playerController;
	public float lifespan = 10f;
	float elapsedTime;

	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (transform.forward * 500f);
		elapsedTime = 0;
	}

	void Update ()
	{
		if (elapsedTime >= lifespan) {
			// Explode();
			Destroy(gameObject);
		}
		elapsedTime += Time.deltaTime;
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag != "Details") {
			if (collision.gameObject.tag == "Player") {
				// weird damage scaling but eh
				playerController.SendMessage ("Damage", Vector3.Magnitude(body.velocity));
			}

			//Explode();
			Destroy (gameObject);
		}
	}
}
