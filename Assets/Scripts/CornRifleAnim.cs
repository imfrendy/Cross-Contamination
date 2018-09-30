using UnityEngine;
using System.Collections;

public class CornRifleAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// Use this for initialization
	void ReloadAnim () {
		GetComponent<Animator> ().Play ("Reload", -1, 0f);
	}
}
