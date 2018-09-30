using UnityEngine;
using System.Collections;

public class Spinny : MonoBehaviour
{
    public GameObject cam;
	
	void Update ()
    {
        cam.transform.Rotate(Vector3.up * 3 * Time.deltaTime);
    }
}
