using UnityEngine;
using System.Collections;

public class EggReload : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ReloadAnim () {
		GetComponent<Animator> ().Play ("Reload", -1, 0f);
	}
}
