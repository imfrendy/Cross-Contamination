using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Revolver : MonoBehaviour
{
    public GameObject raycastStartPoint;
	public GameObject revolverModel;
    public GameObject muzzleFlash;
    public GameObject crosshair;
    public Light muzzleLight;

    // main (not viewmodel) camera
    public Camera camera;
    public GameObject controller;

    // seconds per shot
    public float rateOfFire = .5f;
    float timeSinceShot;

    public LayerMask myLayerMask;

    AudioSource audio;


    public float defaultFOV = 80f;
    public float zoomFOV = 10f;

    CurrentWeapon wep;
    string previousWep;

    public float timeToReload = 100f / 30f;
    public float timeToEquip = 25f / 30f;
    bool reloading;
    bool equipped;
    float equipTimer;
    float reloadTimer;
    System.Random random = new System.Random();

	public float maxDistance = 100f;

    public string weaponName = "revolver";

    // Use this for initialization
    void Start()
    {
        timeSinceShot = rateOfFire;
        audio = GetComponent<AudioSource>();
        wep = GetComponent<CurrentWeapon>();

        equipped = false;
        reloading = false;
        equipTimer = 0;
        reloadTimer = 0;

        muzzleFlash.SetActive(false);
        muzzleLight.GetComponent<Light>().enabled = false;
    }

    void Update()
    {
        muzzleLight.GetComponent<Light>().enabled = false;
        if (wep.currentWeapon == weaponName)
        {
            timeSinceShot += Time.deltaTime;

            if (!equipped)
            {
                // cancel reload when not equipped
                reloading = false;
                reloadTimer = 0;

                if (equipTimer >= timeToEquip)
                {
                    equipped = true;
                    timeSinceShot = rateOfFire;
                }
                else
                {
                    equipTimer += Time.deltaTime;
                }
            }

            if (reloading)
            {
                // reload has finished
                if (reloadTimer >= timeToReload)
                {
                    Reload();
                    reloading = false;
                    timeSinceShot = rateOfFire;
                }
                else if (timeSinceShot >= rateOfFire)
                {
                    reloadTimer += Time.deltaTime;

                    // starting to reload
                    if ((timeSinceShot - Time.deltaTime) < rateOfFire)
                    {
                        BroadcastMessage("ReloadAnim");
                    }
                }
            }

            if (previousWep != wep.currentWeapon)
            {
                equipTimer = 0;

                // show viewmodel
                revolverModel.SetActive(true);
                crosshair.SetActive(true);
            }
            muzzleFlash.SetActive(false);

            if (equipped && !reloading)
            {
                // M1
                if (Time.timeScale == 1)
                {
                    if (Input.GetButton("Fire1") && timeSinceShot >= rateOfFire)
                    {
                        if (wep.GetAmmo(weaponName) >= 1)
                        {
                            FireShot();
                            //Plays sound and activates the muzzle flash
                            int firemode = random.Next(0, 2);

                            muzzleFlash.SetActive(true);
                            muzzleLight.GetComponent<Light>().enabled = true;
                            audio.Play();
                            if ( firemode == 1)
                            {
                                BroadcastMessage("Fire1"); // play anim
                            }
                            else
                            {
                                BroadcastMessage("Fire2"); // play anim
                            }

                            
                        }
                    }
                }

                // when R is pressed or ammo is out, ammo is not full, and it is not currently zoomed
                // queues reload if pressed while in fire cooldown (pulling bolt)
                if ((Input.GetAxis("Reload") == 1 || wep.GetAmmo(weaponName) == 0)
                    && wep.GetAmmo(weaponName) < wep.GetMaxAmmo(weaponName))
                {
                    reloadTimer = 0;
                    reloading = true;

                    if (timeSinceShot >= rateOfFire)
                    {
                        BroadcastMessage("ReloadAnim");
                    }
                }
            }
        }
        else
        {
            if (previousWep != wep.currentWeapon)
            {
                // hide viewmodel
                revolverModel.SetActive(false);
                crosshair.SetActive(true);

            }
        }
        previousWep = wep.currentWeapon;
    }

    void FireShot()
    {
        timeSinceShot = 0;
        Vector3 direction = raycastStartPoint.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        UseAmmo(1);


        // tells the collider that got hit: APPLY DAMAGE
        // arbitrarily long distance as that is necessary to use layermask

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

        hits = Physics.RaycastAll(raycastStartPoint.transform.position, direction, 10000000000000f, myLayerMask);
        if (hits.Length > 0)
        {
            // just initializing the variable so it doesn't complain
            hit = hits[0];
            print(hits.Length);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                print(hits[i].collider);
            }
            // allows for collateral hits until it hits a non-enemy; reverse order due to the nature of RaycastAll
            for (int i = hits.Length - 1; i >= 0 && hits[i].collider.tag == "Enemy"; i--)
            {
                hit = hits[i];
                //print (hit.collider.gameObject);
                // if ur enemy
                if (hit.collider.tag == "Enemy")
                {
                    // maybe make this scale a bit
					float damage = (Mathf.SmoothStep(0, 300, (maxDistance - Vector3.Distance(raycastStartPoint.transform.position, hit.point)) / maxDistance) - 1) / 6;

                    hit.collider.gameObject.SendMessageUpwards("ApplyDamage", damage);
                    // rifle pushback force
                    hit.collider.gameObject.SendMessageUpwards("Push", (raycastStartPoint.transform.TransformDirection(Vector3.forward)).normalized * damage * 15f);
                }
            }
        }
    }

    void Reload()
    {
        // TODO: wait duration of animation, animate cornrifle model
        wep.ResetAmmo(weaponName);
    }

    void UseAmmo(int ammo = 1)
    {
        wep.UseAmmo(weaponName, ammo);
    }
}