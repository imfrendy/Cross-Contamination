using UnityEngine;
using System.Collections;

public class RevolverAnim : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void ReloadAnim()
    {
        GetComponent<Animator>().Play("Reload", -1, 0f);
    }

    void Fire1()
    {
        GetComponent<Animator>().Play("Fire1", -1, 0f);
    }

    void Fire2()
    {
        GetComponent<Animator>().Play("Fire2", -1, 0f);
    }
}

