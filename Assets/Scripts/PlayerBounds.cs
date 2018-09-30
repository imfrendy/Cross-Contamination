// DONT USE THIS!!!!!
// Replaced with PlayerAttributes.cs

using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour {
	Vector3 origin;

	// step this distance from origin and you're dead, kiddo
	public float boundsRadius = 150f;

	// Use this for initialization
	void Start () {
		origin = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, origin) >= boundsRadius) {
			// implement death timer

			print ("You are out of bounds at point " + transform.position);
		} else {
			//print ("FPS: " + (1 / Time.deltaTime));
		}
	}
}
