using UnityEngine;
using System.Collections;

public class EggLauncher : MonoBehaviour {
	// player camera
	public GameObject projectileStartPoint;

	public GameObject EggLauncherModel;
	public GameObject Egg;

	// seconds per shot
	public float rateOfFire = 1f;
	float timeSinceShot;

	AudioSource audio;

	CurrentWeapon wep;
	string previousWep;

	public string weaponName = "EggLauncher";

	public float timeToReload = 47f / 30f;
	public float timeToEquip = 0.5f;
	bool reloading;
	bool equipped;
	float equipTimer;
	float reloadTimer;

	// Use this for initialization
	void Start () {
		timeSinceShot = rateOfFire;
		audio = GetComponent<AudioSource>();
		wep = GetComponent<CurrentWeapon> ();

		equipped = false;
		reloading = false;
		equipTimer = 0;
		reloadTimer = 0;
	}

	void Update()
	{
		if (wep.currentWeapon == weaponName) {
			timeSinceShot += Time.deltaTime;

			if (!equipped) {
				// cancel reload when not equipped
				reloading = false;
				reloadTimer = 0;

				if (equipTimer >= timeToEquip) {
					equipped = true;
					timeSinceShot = rateOfFire;
				} else {
					equipTimer += Time.deltaTime;
				}
			}

			// if R key press has been queued
			if (reloading) {
				if (reloadTimer >= timeToReload) {
					Reload ();
					reloading = false;
					timeSinceShot = rateOfFire;
				} else if (timeSinceShot >= rateOfFire) {
					reloadTimer += Time.deltaTime;

					// starting to reload
					if ((timeSinceShot - Time.deltaTime) < rateOfFire) {
						BroadcastMessage ("ReloadAnim");
					}
				}
			}

			if (previousWep != wep.currentWeapon) {
				// show viewmodel
				equipTimer = 0;
				EggLauncherModel.SetActive(true);
			}

			if (equipped && !reloading) {
				// M1
				if (Time.timeScale == 1) {
					if (Input.GetButton ("Fire1") && timeSinceShot >= rateOfFire) {
						if (wep.GetAmmo (weaponName) >= 1) {
							FireShot ();
						}
					}
				}

				if ((Input.GetAxis ("Reload") == 1 || wep.GetAmmo(weaponName) == 0)
					&& wep.GetAmmo (weaponName) < wep.GetMaxAmmo (weaponName)) {

					reloadTimer = 0;
					reloading = true;

					if (timeSinceShot >= rateOfFire) {
						BroadcastMessage ("ReloadAnim");
					}
				}
			}
		} else {
			if (previousWep != wep.currentWeapon) {
				// hide viewmodel
				EggLauncherModel.SetActive(false);
				equipped = false;
			}
		}
		previousWep = wep.currentWeapon;
	}

	void FireShot() {
		
		timeSinceShot = 0;

		UseAmmo ();

		// forwards, and down
		Vector3 startPt = projectileStartPoint.transform.position
		                 + projectileStartPoint.transform.forward * 1f
		//+ projectileStartPoint.transform.TransformDirection(Vector3.right) * 0.2f
		                 //- projectileStartPoint.transform.up * 0.2f
			;
		// kick
		EggLauncherModel.SendMessage ("startKick");

		Quaternion rotation = projectileStartPoint.transform.rotation;

		Egg.SetActive (true);
		Instantiate (Egg, startPt, rotation);
		Egg.SetActive (false);

		audio.Play ();
	}

	void Reload() {
		// TODO: wait duration of animation, animate cornrifle model
		wep.ResetAmmo(weaponName);
	}

	void UseAmmo(int ammo = 1)
	{
		wep.UseAmmo (weaponName, ammo);
	}
}

