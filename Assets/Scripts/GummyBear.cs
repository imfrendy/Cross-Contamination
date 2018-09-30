using UnityEngine;
using System.Collections;

public class GummyBear : MonoBehaviour {
	public GameObject playerController;
	public GameObject gel;
    public GameObject particle;

    Rigidbody gb;

	public float maxHealth = 60f;
	float health;

	public float moveSpeed = 4f;

	public float maxDistance = 30f;
	public float shootDistance = 20f;

	public float ROF = 3f;
	float timeSinceShot;


	RaycastHit hit;

	float timeSinceRotate;
	public float timeToRotate = 3f;
	bool turning;
	public float turnDuration = 1f;
	float turnDegs;

	public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		gb = GetComponent<Rigidbody> ();
		turning = false;
		gb.centerOfMass = new Vector3 (0, 1f, 0);
		timeSinceShot = ROF;
	}

	// Update is called once per frame
	void Update () {
        //Stops constant explosions
        particle.SetActive(false);

        if (transform.position.y < -10)
			Die ();
		
		// eye level
		if (Physics.Linecast (transform.position + 2.8f*transform.up, playerController.transform.position, out hit, layerMask)) {
			//Debug.DrawLine (transform.position + 2.8f*transform.up, playerController.transform.position);
			// if bear can see player & within range
			if (hit.collider.tag == "Player" &&
			    Vector3.Distance (transform.position, playerController.transform.position) <= maxDistance) {
				//transform.LookAt (playerController.transform);
				Vector3 targetDir = playerController.transform.position - transform.position;
				float step = 2f * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);


				// here's my really convoluted solution to making the gummybear turn. yay!
				// keep old x rot
				float oldX = transform.rotation.eulerAngles.x;
				Quaternion oldRot = transform.rotation;
				transform.rotation = Quaternion.LookRotation (newDir);
				Vector3 temp = transform.eulerAngles;
				temp.x = oldX;
				transform.rotation = oldRot;

				gb.MoveRotation (Quaternion.Euler(temp));

				// shoot when in range
				if (Vector3.Distance (transform.position, playerController.transform.position) <= shootDistance &&
				    timeSinceShot >= ROF) {
					//spawn in front of bear; locks to player
					Vector3 spawnPos = transform.position + transform.forward * 1.5f + transform.up * 2.0f;

					Quaternion angleToPlayer = Quaternion.LookRotation (playerController.transform.position - spawnPos + new Vector3 (0, 0.5f, 0));

					gel.SetActive (true);
					Instantiate (gel, spawnPos, Quaternion.Euler(angleToPlayer.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
					gel.SetActive (false);

					timeSinceShot = 0;
				}
			} else {
				if (timeSinceRotate >= timeToRotate) {
					if (timeSinceRotate - timeToRotate <= turnDuration) {
						if (!turning) {
							turnDegs = Random.Range (-180f, 180f);
							turning = true;
						}
						gb.MoveRotation(gb.rotation * Quaternion.Euler(new Vector3 (0, turnDegs * Time.deltaTime, 0)));
					} else {
						turning = false;
						timeSinceRotate = 0;
					}

				}
			}
		} else {
			print ("Linecast is not working");
		}

		//move when it can't shoot
		if (Vector3.Distance (transform.position, playerController.transform.position) > shootDistance) {
			gb.MovePosition (transform.position + transform.forward * moveSpeed * Time.deltaTime);
		}

		timeSinceRotate += Time.deltaTime;
		timeSinceShot += Time.deltaTime;
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
        // die and explode
        particle.SetActive(true);
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
        particle.SetActive(false);
    }

    void AddEggExplosion(Vector3 sourcePoint)
    {
        // 40.0f = radius, 5000f = force
        gb.AddExplosionForce(1000f, sourcePoint, 10.0f);

        float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance(sourcePoint, transform.position)) / 10.0f);
        ApplyDamage(damage);
    }

    void AddPineappleExplosion(Vector3 sourcePoint)
    {
        // 40.0f = radius, 5000f = force
        gb.AddExplosionForce(1000f, sourcePoint, 20.0f);

        float damage = Mathf.SmoothStep(0, 100, (20.0f - Vector3.Distance(sourcePoint, transform.position)) / 20.0f);
        ApplyDamage(damage);
    }

    void Push (Vector3 forceVector)
    {
        gb.AddForce(forceVector);
    }

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Terrain") {
			//gb.velocity *= (0.9f * Time.deltaTime);
		}
	}

}