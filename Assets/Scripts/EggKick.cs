using UnityEngine;
using System.Collections;

public class EggKick : MonoBehaviour {

	// 0 = stopped; 1 = going up; 2 = going down
	int kickStatus; 


	public float kickDuration = .3f; // MUST BE LOWER THAN WEAPON ROF / 2! THIS IS HALF TIME
	float kickTimer;

	// degrees of rotation
	public float totalRot = -10f;

	Quaternion startRot;
	Quaternion newRot;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (kickStatus == 1) {
			kickTimer += Time.deltaTime;

			if (kickTimer > kickDuration) {
				kickStatus = 2;
				kickTimer = 0;
				transform.localRotation = newRot;
			} 
			else {
				transform.localRotation = Quaternion.Lerp (startRot, newRot, kickTimer / kickDuration);
			}

		}
		else if (kickStatus == 2) {
			kickTimer += Time.deltaTime;

			if (kickTimer > kickDuration) {
				kickStatus = 0;
				transform.localRotation = startRot;
			} 
			else {
				transform.localRotation = Quaternion.Lerp (newRot, startRot, kickTimer / kickDuration);
			}
		}

	}

	void startKick ()
	{
		
		if (kickStatus == 0) {
			kickStatus = 1;
			kickTimer = 0;
			startRot = transform.localRotation;
			newRot = Quaternion.Euler (transform.localRotation.eulerAngles.x + totalRot,
				transform.localRotation.eulerAngles.y, 
				transform.localRotation.eulerAngles.z);
		}
	}
}
