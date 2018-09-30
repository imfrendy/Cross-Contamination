using UnityEngine;
using System.Collections;

public class BootCollider : MonoBehaviour
{
    public GameObject FPSController;
    public GameObject Canister;
    void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Canister.SetActive(false);
            FPSController.SendMessage("showJump");
            print("jumper");
        }
    }
}
