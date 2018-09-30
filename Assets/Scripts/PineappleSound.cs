using UnityEngine;
using System.Collections;

public class PineappleSound : MonoBehaviour
{
    AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            audio.Play();
        }
        else
        {

        }
    }
}
