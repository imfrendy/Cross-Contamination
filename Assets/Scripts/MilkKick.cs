using UnityEngine;
using System.Collections;

public class MilkKick : MonoBehaviour {
	bool isKicking; 
	float kickTimer;
	public float transitionDuration = .01f;

	public float totalDist = 0.003f;

	Vector3 startPos;
	Vector3 newPos;

	// Use this for initialization
	void Start () {
		isKicking = false;
		startPos = transform.localPosition;
		newPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (isKicking) {
			if (transform.localPosition.y != newPos.y) {
				transform.localPosition = new Vector3(transform.localPosition.x, 
					Mathf.Lerp (startPos.y, newPos.y, kickTimer > 1 ? 1 : kickTimer / transitionDuration),
						transform.localPosition.z);
			}
		} else {
			if (transform.localPosition.y != startPos.y) {
				transform.localPosition = new Vector3(transform.localPosition.x, 
					Mathf.Lerp (newPos.y, startPos.y, kickTimer > 1 ? 1 : kickTimer / transitionDuration),
						transform.localPosition.z);
			}
		}
		kickTimer += Time.deltaTime;
	}

	void startKick ()
	{
		if (!isKicking) {
			kickTimer = 0;
			isKicking = true;
			newPos = new Vector3(transform.localPosition.x, 
				startPos.y + totalDist, 
				transform.localPosition.z);
		}

	}
	void endKick()
	{
		if (isKicking) {
			kickTimer = 0;
			isKicking = false;
		}
	}
}
