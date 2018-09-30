using UnityEngine;
using System.Collections;

public class CubeEnemyScript : MonoBehaviour {
	public GameObject playerController;

	public float maxHealth = 100f;
	float health;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0)
			Die ();
	}

	void ApplyDamage (float damage)	{
		this.health -= damage;

		// for entertainment purposes
		transform.localScale = new Vector3(5 * health / maxHealth, 5 * health / maxHealth, 5 * health / maxHealth);
		if (health <= 0) {
			Die ();
		}
	}

	void Die ()
	{
		// die
		Destroy (gameObject);
	}

	// currently the cube will explode away if it touches the player
	void CubeExplosion(GameObject player)
	{
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddExplosionForce(1000.0f, player.transform.position, 200.0f);
	}

	void AddEggExplosion (Vector3 sourcePoint)
	{
		// 40.0f = radius, 5000f = force
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddExplosionForce(1000f, sourcePoint, 10.0f);

		float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance (sourcePoint, transform.position) ) / 10.0f) - 1;
		ApplyDamage (damage);
	}

	void AddPineappleExplosion (Vector3 sourcePoint)
	{
		// 40.0f = radius, 5000f = force
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddExplosionForce(1000f, sourcePoint, 20.0f);

		float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance (sourcePoint, transform.position) ) / 10.0f) - 1;
		ApplyDamage (damage);
	}

	void RiflePush (Vector3 forceVector)
	{
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce (forceVector);
	}
}
