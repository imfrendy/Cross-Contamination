using UnityEngine;
using System.Collections;

public class MilkThrower : MonoBehaviour {
	public GameObject projectileStartPoint;
	public GameObject milkModel;
	public GameObject MilkParticle;

	// seconds per shot
	public float rateOfFire = 0.05f;
	float timeSinceShot;

	AudioSource audio;

	CurrentWeapon wep;
	string previousWep;

	public string weaponName = "MilkThrower";

	// Use this for initialization
	void Start () {
		timeSinceShot = rateOfFire;
		audio = GetComponent<AudioSource>();
		wep = GetComponent<CurrentWeapon> ();
	}

	void Update()
	{
		if (wep.currentWeapon == weaponName) {
			if (previousWep != wep.currentWeapon) {
				// show viewmodel
				milkModel.SetActive(true);
			}
			// M1
			if (Time.timeScale == 1) {
				if (Input.GetButton ("Fire1") && timeSinceShot >= rateOfFire) {
					FireShot ();
				}
			}

			timeSinceShot += Time.deltaTime;

			if (timeSinceShot >= rateOfFire * 2) {
				milkModel.SendMessage ("endKick");
			}

			if (Input.GetAxis ("Reload") == 1) {
				// add exception to if currently reloading or at max ammo
				Reload ();
			}

		} else {
			if (previousWep != wep.currentWeapon) {
				// hide viewmodel
				milkModel.SetActive(false);
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
			                 + projectileStartPoint.transform.TransformDirection (Vector3.right) * 0.35f
			                 - projectileStartPoint.transform.up * 0.35f
							 ;
			// sorta redundant
			milkModel.SendMessage ("startKick");

			Quaternion rotation = projectileStartPoint.transform.rotation *
			                     Quaternion.Euler (new Vector3 (Random.Range (-4f, 4f), Random.Range (-6f, 6f), 10f)); // aim a little more to the left

			MilkParticle.SetActive (true);
			Instantiate (MilkParticle, startPt, rotation);
			MilkParticle.SetActive (false);

			audio.Play ();
            if(wep.GetAmmo(weaponName)==0)
            {
                Reload();
            }
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
