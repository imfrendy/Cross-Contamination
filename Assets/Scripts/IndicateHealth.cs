using UnityEngine;
using System.Collections;

public class IndicateHealth : MonoBehaviour {
	Color[] startColors;
	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();

		startColors = new Color[rend.materials.Length];
		for (int i = 0; i < rend.materials.Length; i++) {
			startColors[i] = rend.materials[i].color;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Indicate (float healthPercent) {

		for (int i = 0; i < rend.materials.Length; i++) {
			rend.materials[i].color = Color.Lerp(Color.red, startColors[i], healthPercent);
		}
	}
}
