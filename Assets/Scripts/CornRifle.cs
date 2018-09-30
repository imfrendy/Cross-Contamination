using UnityEngine;
using System.Collections;

public class CornRifle : MonoBehaviour {
	public GameObject raycastStartPoint;
	public GameObject cornRifleModel;
	public GameObject cornKernel;
    public GameObject muzzleFlash1;
    public GameObject muzzleFlash2;
    public Light muzzleLight;

	// seconds per shot
	public float rateOfFire = 0.15f;
	float timeSinceShot;

	public float maxDistance = 50f;

	public float spreadDegrees = 5f;

	public LayerMask myLayerMask;

	AudioSource audio;

	CurrentWeapon wep;
	string previousWep;

	public string weaponName = "CornRifle";

	public float timeToReload = 70f/30f;
	public float timeToEquip = 16f/30f;
	bool reloading;
	bool equipped;
	float equipTimer;
	float reloadTimer;

	// Use this for initialization
	void Start () {
		timeSinceShot = rateOfFire;
		audio = GetComponent<AudioSource>();
		wep = GetComponent<CurrentWeapon> ();
        muzzleFlash1.SetActive(false);
        muzzleFlash2.SetActive(false);
        muzzleLight.GetComponent<Light>().enabled = false;

		equipped = false;
		reloading = false;
		equipTimer = 0;
		reloadTimer = 0;
    }

	void Update()
	{

        int randompic = Random.Range(0, 2);
        muzzleFlash1.SetActive(false);
        muzzleFlash2.SetActive(false);
        muzzleLight.GetComponent<Light>().enabled = false;
        if (wep.currentWeapon == weaponName) {
			timeSinceShot += Time.deltaTime;

            if (previousWep != wep.currentWeapon)
            {
                // show viewmodel
                equipTimer = 0;
                cornRifleModel.SetActive(true);
            }

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


			// M1
			if (equipped && !reloading) {
				if (Time.timeScale == 1) {
					if (Input.GetButton ("Fire1") && timeSinceShot >= rateOfFire) {
						if (wep.GetAmmo (weaponName) >= 6) {
                            FireShot ();
							//Plays sound and activates the muzzle flash
                    
							if (randompic == 0) {
								muzzleFlash1.SetActive (true);
							}
							if (randompic == 1) {
								muzzleFlash2.SetActive (true);
							}
							muzzleLight.GetComponent<Light> ().enabled = true;

                            if (wep.GetAmmo(weaponName) == 0)
                            {
                                BroadcastMessage("ReloadAnim");
                                reloadTimer = 0;
                                reloading = true;
                            }
                        }
					}
				}

				// reload when R pressed or out of ammo
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
				cornRifleModel.SetActive(false);
				equipped = false;
			}
		}
		previousWep = wep.currentWeapon;
	}

	void FireShot() {
    	timeSinceShot = 0;
        Vector3 direction = raycastStartPoint.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // 6 barrels, lol
        UseAmmo(6);



        // tells the collider that got hit: APPLY DAMAGE
        for (int i = 0; i < 6; i++)
        {
            // bullet spread
            direction = Quaternion.Euler(Random.Range(-1 * spreadDegrees, spreadDegrees),
                Random.Range(-1 * spreadDegrees, spreadDegrees), 0) * direction;

            if (Physics.Raycast(raycastStartPoint.transform.position, direction, out hit, maxDistance, myLayerMask)) {

                // if ur enemy
                if (hit.collider.tag == "Enemy") {
                    float damage = (Mathf.SmoothStep(5, 40, (maxDistance - Vector3.Distance(raycastStartPoint.transform.position, hit.point)) / maxDistance) - 1) / 6;
                    hit.collider.gameObject.SendMessageUpwards("ApplyDamage", damage);

                    // rifle pushback force
                    hit.collider.gameObject.SendMessageUpwards("Push", (raycastStartPoint.transform.TransformDirection(Vector3.forward)).normalized * damage * 15f);
                }

                cornKernel.SetActive(true);
                Instantiate(cornKernel, hit.point + new Vector3(Random.Range(-0.2f, 0.2f), 0.01f, Random.Range(-0.2f, 0.2f)), transform.rotation);
                cornKernel.SetActive(false);
            }
        }

        cornRifleModel.SendMessage("startKick");
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
