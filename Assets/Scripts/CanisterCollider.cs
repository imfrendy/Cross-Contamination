using UnityEngine;
using System.Collections;

public class CanisterCollider : MonoBehaviour
{
    public GameObject FPSController;
    public GameObject Canister;
	void OnTriggerEnter (Collider collision)
    {
		if (collision.gameObject.tag == "Player")
        {
            Canister.SetActive(false);
            FPSController.SendMessage("HealUnlock");
			print ("Heal unlocked");
        }
    }
}
