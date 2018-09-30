using UnityEngine;
using System.Collections;

public class PineappleLauncher : MonoBehaviour {
	// player camera
	public GameObject projectileStartPoint;

	public GameObject PineappleLauncherModel;
	public GameObject Pineapple;

	// seconds per shot
	public float rateOfFire = 1.5f;
	float timeSinceShot;

	AudioSource audio;

	CurrentWeapon wep;
	string previousWep;

	public string weaponName = "PineappleLauncher";

	public float timeToReload = 1f;
	public float timeToEquip = 21f/30f;
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
				PineappleLauncherModel.SetActive(true);
			}

			if (equipped && !reloading) {
				// M1
				if (Time.timeScale == 1) {
					if (Input.GetButton ("Fire1") && timeSinceShot >= rateOfFire) {
						FireShot ();
					}
				}

				// if key R pressed and not at max
				// or at 0 ammo
				if ((Input.GetAxis ("Reload") == 1 && wep.GetAmmo (weaponName) < wep.GetMaxAmmo (weaponName)) 
					|| wep.GetAmmo (weaponName) == 0) {
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
				PineappleLauncherModel.SetActive(false);
				equipped = false;
			}
		}
		previousWep = wep.currentWeapon;
	}

	void FireShot() {
		if (wep.GetAmmo (weaponName) >= 1) {
			timeSinceShot = 0;

			UseAmmo ();

			// forwards, and down
			Vector3 startPt = projectileStartPoint.transform.position
			                  + projectileStartPoint.transform.forward * 2.5f
			                  //+ projectileStartPoint.transform.TransformDirection (Vector3.right) * 0.3f;
								//- projectileStartPoint.transform.up * 0.2f
				;
		
			// ROCKET LAUNCHERS DON'T KICK

			// remember to put random rotation on the actual particle after applying force
			//Quaternion rotation = Quaternion.identity;

			Pineapple.SetActive (true);
			Instantiate (Pineapple, startPt, projectileStartPoint.transform.rotation);
			Pineapple.SetActive (false);

			audio.Play ();
		}
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
