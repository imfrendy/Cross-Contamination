// this will handle ammo as well as weapon switching

using UnityEngine;
using System.Collections;

public class CurrentWeapon : MonoBehaviour {
	// public so other objects can access
	public string currentWeapon = "";
	public string[] inventory = { "CornRifle", "MilkThrower", "EggLauncher", "PineappleLauncher", "ButterKnife", "CarrotBarrett", "Revolver" };

	public Canvas HUD;

	// 800 kernels per ear of corn
	public int[] maxAmmo = { 402, 200, 6, 4, 0, 8, 6 };
	private int []currentAmmo;

	// Use this for initialization
	void Start () {
		// switch to first weapon if there is not a given weapon
		if (currentWeapon == "") {
			ChangeWeapon (inventory [0]);
		}
		currentAmmo = new int[maxAmmo.Length];
		for (int i = 0; i < maxAmmo.Length; i++) {
			currentAmmo [i] = maxAmmo [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		// change weapon
		if (Time.timeScale == 1) {
			// keys 1-9
			for (int i = 1; i <= 9; i++) {
				if (Input.GetKeyDown (i.ToString ())) {
					if (i <= inventory.Length) {
						ChangeWeapon (inventory [i - 1]);
						HUD.SendMessage ("DisplayWeapon", currentWeapon);
					}
				}
			}

			if (Input.GetAxisRaw ("Mouse ScrollWheel") < 0) {
				if (System.Array.IndexOf<string> (inventory, currentWeapon) == 0) {
					ChangeWeapon (inventory [inventory.Length - 1]);
					HUD.SendMessage ("DisplayWeapon", currentWeapon);
				} else {
					ChangeWeapon (inventory [System.Array.IndexOf<string> (inventory, currentWeapon) - 1]);
					HUD.SendMessage ("DisplayWeapon", currentWeapon);
				}
			}

			if (Input.GetAxisRaw ("Mouse ScrollWheel") > 0) {
				if (System.Array.IndexOf<string> (inventory, currentWeapon) >= inventory.Length - 1) {
					ChangeWeapon (inventory [0]);
					HUD.SendMessage ("DisplayWeapon", currentWeapon);
				} else {
					ChangeWeapon (inventory [System.Array.IndexOf<string> (inventory, currentWeapon) + 1]);
					HUD.SendMessage ("DisplayWeapon", currentWeapon);
				}
			}
		}

        if (currentWeapon != "ButterKnife") {
			HUD.SendMessage ("DisplayAmmo", currentAmmo [System.Array.IndexOf<string> (inventory, currentWeapon)] + "/"
			+ maxAmmo [System.Array.IndexOf<string> (inventory, currentWeapon)]);
		} else {
			HUD.SendMessage ("DisplayAmmo", "");
		}
	}

	void ChangeWeapon (string newWeapon)
	{
		currentWeapon = newWeapon;
	}


	public void UseAmmo(string weapon, int ammoUsed)
	{
		// get index and subtract ammo
		if (System.Array.IndexOf<string>(inventory, weapon) != -1) {
			if (currentAmmo [System.Array.IndexOf<string>(inventory, weapon)] > 0) {
				currentAmmo [System.Array.IndexOf<string>(inventory, weapon)] -= ammoUsed;
			} else {
				// this should never be reached
				print ("YOU'RE OUT OF AMMO");
			}
		} else {
			print ("THAT WEAPON IS NOT IN THE INVENTORY!!!!");
		}
	}

	public int GetAmmo (string weapon) {
		if (System.Array.IndexOf<string>(inventory, weapon) != -1) {
			return currentAmmo [System.Array.IndexOf<string> (inventory, weapon)];
		} else {
			// should not happen
			print ("THAT WEAPON IS NOT IN THE INVENTORY!!!!");
			return -1;
		}
	}

	public int GetMaxAmmo (string weapon)
	{
		if (System.Array.IndexOf<string>(inventory, weapon) != -1) {
			return maxAmmo [System.Array.IndexOf<string> (inventory, weapon)];
		} else {
			// should not happen
			print ("THAT WEAPON IS NOT IN THE INVENTORY!!!!");
			return -1;
		}
	}

	public void ResetAmmo (string weapon) {
		if (System.Array.IndexOf<string>(inventory, weapon) != -1) {
			currentAmmo [System.Array.IndexOf<string> (inventory, weapon)] = maxAmmo [System.Array.IndexOf<string> (inventory, weapon)];
		} else {
			print ("THAT WEAPON IS NOT IN THE INVENTORY!!!!");
		}
	}
}
