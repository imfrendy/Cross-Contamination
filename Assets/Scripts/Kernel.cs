using UnityEngine;
using System.Collections;

public class Kernel : MonoBehaviour {
	Rigidbody body;

	float disableTimer;
	bool hasTouchedGround;
	public float timeToDisable = 1f;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		body.AddForce (transform.up * 20f);
		transform.rotation = Random.rotation;

		hasTouchedGround = false;
		disableTimer = 0f;
	}

	void Update() {
		if (hasTouchedGround) {
			if (Vector3.Magnitude (body.velocity) <= 0.2 && disableTimer > timeToDisable) {
				body.isKinematic = true;
			}
			disableTimer += Time.deltaTime;
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		// if kernel hits the ground OR a disabled world detail
		if (collision.gameObject.tag == "Terrain" || 
			(collision.gameObject.tag == "Details" && collision.gameObject.GetComponent<Rigidbody> ().isKinematic == true)) {
			if (!hasTouchedGround) {
				hasTouchedGround = true;
			}
		}
	}
}
