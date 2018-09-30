using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarrotBarrett : MonoBehaviour
{
    public GameObject raycastStartPoint;
	public GameObject barrettModelParent;
    public GameObject barrettModel1;
    public GameObject barrettModel2;
    public GameObject barrettModel3;
    Renderer barretRend1;
    Renderer barretRend2;
    Renderer barretRend3;
    public GameObject carrot;
    public GameObject muzzleFlash;
    public GameObject crosshair;
    public Light muzzleLight;
    public Image zoomCrosshair;
    public GameObject zoomCrosshairObject;

    // main (not viewmodel) camera
    public Camera camera;
    public GameObject controller;

    // seconds per shot
    public float rateOfFire = 2f;
    float timeSinceShot;

    // only when unzoomed
    public float spreadDegrees = 20f;

    public LayerMask myLayerMask;

    AudioSource audio;



    bool zoomed;
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

    public string weaponName = "CarrotBarrett";

    // Use this for initialization
    void Start()
    {
        barretRend1 = barrettModel1.GetComponent<Renderer>();
        barretRend2 = barrettModel2.GetComponent<Renderer>();
        barretRend3 = barrettModel3.GetComponent<Renderer>();
        zoomed = false;
        zoomCrosshair.enabled = false;
        zoomCrosshairObject.SetActive(false);

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
            crosshair.SetActive(false);
            timeSinceShot += Time.deltaTime;
            zoomCrosshairObject.SetActive(true);

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
                barrettModel1.SetActive(true);
                barrettModel2.SetActive(true);
                barrettModel3.SetActive(true);
				barrettModelParent.SetActive (true);
                barretRend1.enabled = true;
                barretRend2.enabled = true;
                barretRend3.enabled = true;
                crosshair.SetActive(false);
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

                            muzzleFlash.SetActive(true);
                            muzzleLight.GetComponent<Light>().enabled = true;
                            audio.Play();

                            // zoom out after shot
                            if (zoomed)
                            {
                                controller.SendMessage("Zoom");
                                zoomed = false;
                                DoZoom(false, defaultFOV, zoomFOV);
                            }
                            crosshair.SetActive(false);

                            BroadcastMessage("Fire"); // play anim
                        }
                    }
                    // only allow zoom when ready to fire (i.e. not playing bolt anim)
                    // add exception for when zoomed
                    if (Input.GetButtonDown("Fire2") && timeSinceShot >= rateOfFire)
                    {
                        zoomed = !zoomed;
                        DoZoom(zoomed, defaultFOV, zoomFOV);

                        // send zoom state to the controller
                        controller.SendMessage("Zoom");

                        // animate the barrett -- not gonna use
                        //if (zoomed) {
                        //	BroadcastMessage ("Scope");
                        //} else {
                        //	BroadcastMessage ("Unscope");
                        //}
                    }
                }

                // when R is pressed or ammo is out, ammo is not full, and it is not currently zoomed
                // queues reload if pressed while in fire cooldown (pulling bolt)
                if ((Input.GetAxis("Reload") == 1 || wep.GetAmmo(weaponName) == 0)
                    && wep.GetAmmo(weaponName) < wep.GetMaxAmmo(weaponName) && !zoomed)
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
                barrettModel1.SetActive(false);
                barrettModel2.SetActive(false);
                barrettModel3.SetActive(false);
				barrettModelParent.SetActive (false);
                crosshair.SetActive(true);

                // unzoomed if zoomed in
                if (zoomed)
                {
                    zoomed = !zoomed;
                    DoZoom(zoomed, defaultFOV, zoomFOV);

                    // send zoom state to the controller
                    controller.SendMessage("Zoom");
                }

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

        // bullet spread when unscoped
        if (!zoomed)
        {
            direction = Quaternion.Euler(Random.Range(-1 * spreadDegrees, spreadDegrees),
                Random.Range(-1 * spreadDegrees, spreadDegrees), 0) * direction;
        }

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
                    float damage = 300f;

                    hit.collider.gameObject.SendMessageUpwards("ApplyDamage", damage);
                    // rifle pushback force
                    hit.collider.gameObject.SendMessageUpwards("Push", (raycastStartPoint.transform.TransformDirection(Vector3.forward)).normalized * damage * 35f);
                }
            }
            carrot.SetActive(true);
            Instantiate(carrot, hit.point + new Vector3(Random.Range(-0.2f, 0.2f), 0.01f, Random.Range(-0.2f, 0.2f)), transform.rotation);
            carrot.SetActive(false);
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

    void DoZoom(bool zoom, float FOV, float zoomFOV)
    {
        // todo: show/hide viewmodel without renderer enabled

        if (zoom)
        {
            zoomCrosshair.enabled = true;
            camera.fieldOfView = zoomFOV;
            barretRend1.enabled = false;
            barretRend2.enabled = false;
            barretRend3.enabled = false;
            muzzleFlash.transform.Translate(Vector3.forward * 3.708f);
        }

        else
        {
            zoomCrosshair.enabled = false;
            camera.fieldOfView = defaultFOV;
            barretRend1.enabled = true;
            barretRend2.enabled = true;
            barretRend3.enabled = true;
            muzzleFlash.transform.Translate(Vector3.back * 3.708f);
        }
    }
}