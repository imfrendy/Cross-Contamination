using UnityEngine;
using System.Collections;

public class Kick : MonoBehaviour {
	bool isKicking; 
	public float kickDuration = .01f; // MUST BE LOWER THAN WEAPON ROF!
	float kickTimer;

	public float totalDist = 0.003f;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isKicking) {
			kickTimer += Time.deltaTime;
			if (kickTimer > kickDuration) {
				isKicking = false;
				transform.localPosition = startPos;
			} 
			else {
				transform.localPosition = new Vector3(transform.localPosition.x, 
					Mathf.SmoothStep(startPos.y + totalDist, startPos.y, kickTimer / kickDuration), 
					transform.localPosition.z);
			}
		}
	}

	void startKick ()
	{
		if (!isKicking) {
			isKicking = true;
			kickTimer = 0;
			startPos = transform.localPosition;
			transform.localPosition = new Vector3(transform.localPosition.x, 
				startPos.y + totalDist, 
				transform.localPosition.z);
		}
	}
}
