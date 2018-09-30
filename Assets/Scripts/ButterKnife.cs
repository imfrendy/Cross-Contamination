using UnityEngine;
using System.Collections;

public class ButterKnife : MonoBehaviour {
	public GameObject raycastStartPoint;
	public GameObject butterKnifeModel;

	// seconds per shot
	public float rateOfFire = 1f;
	float timeSinceSwing;

	public float hitRadius = 0.5f;

	public float maxDistance = 1.5f;

	public LayerMask myLayerMask;

	AudioSource audio;

	CurrentWeapon wep;
	string previousWep;

	public string weaponName = "ButterKnife";

	bool swinging;
	public float totalSwingTime = 0.2f;
	float swingTime;

	// Use this for initialization
	void Start () {
		timeSinceSwing = rateOfFire;
		audio = GetComponent<AudioSource>();
		wep = GetComponent<CurrentWeapon> ();
		swinging = false;
		swingTime = 0;
	}

	void Update()
	{
		if (wep.currentWeapon == weaponName) {
			if (previousWep != wep.currentWeapon) {
				// show viewmodel
				butterKnifeModel.SetActive(true);
			}
			// M1
			if (Time.timeScale == 1) {
				if (Input.GetButton ("Fire1") && timeSinceSwing >= rateOfFire) {
					if (!swinging) {
						swinging = true;
						swingTime = 0;
						BroadcastMessage ("startSwing");
					}
				}
			}

			// when swing has gone through
			if (swinging && swingTime >= totalSwingTime) {
				Swing ();
				audio.Play();
			}

			timeSinceSwing += Time.deltaTime;

			if (swinging) {
				swingTime += Time.deltaTime;
			}
		} else {
			if (previousWep != wep.currentWeapon) {
				// hide viewmodel
				butterKnifeModel.SetActive(false);
			}
		}
		previousWep = wep.currentWeapon;
	}

	void Swing() {
		timeSinceSwing = 0;
		Vector3 direction = raycastStartPoint.transform.TransformDirection(Vector3.forward);
		RaycastHit hit;

		// tells the collider that got hit: APPLY DAMAGE
		if (Physics.SphereCast(raycastStartPoint.transform.position, hitRadius, direction, out hit, maxDistance, myLayerMask)) {

			// if ur enemy
			if (hit.collider.tag == "Enemy") {
				float damage = Mathf.SmoothStep(200, 100, (maxDistance - Vector3.Distance(raycastStartPoint.transform.position, hit.point)) / maxDistance) - 1;
				hit.collider.gameObject.SendMessageUpwards("ApplyDamage", damage);

				// pushback force -- a lot more than the corn rifle
				hit.collider.gameObject.SendMessageUpwards("Push", (raycastStartPoint.transform.TransformDirection(Vector3.forward)).normalized * damage * 15f);
			}
		}
		swinging = false;
	}
}
