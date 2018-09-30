using UnityEngine;
using System.Collections;

public class MilkParticle : MonoBehaviour {
	Rigidbody body;

	public float lifespan = 10f;
	float elapsedTime;

	float startScale;

	Vector3 lastPos;

	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (transform.forward * 10f);
		body.AddForce (transform.up * 2f);
		startScale = transform.localScale.x;
		elapsedTime = 0;
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (elapsedTime > lifespan) {
			Destroy (gameObject);
		}
		else
		{
			transform.localScale = new Vector3(Mathf.Lerp(startScale, 0, elapsedTime / lifespan),
				Mathf.Lerp(startScale, 0, elapsedTime / lifespan),
				Mathf.Lerp(startScale, 0, elapsedTime / lifespan));
		}
		elapsedTime += Time.deltaTime;
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Enemy") {
			// weird damage scaling but eh
			collision.gameObject.SendMessageUpwards("ApplyDamage", transform.localScale.x * 50f);
		}

		else if (collision.gameObject.tag == "Terrain") {
			// add splash effect
		}
	}
}
