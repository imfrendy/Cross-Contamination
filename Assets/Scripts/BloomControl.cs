using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class BloomControl : MonoBehaviour {
    Bloom bloom;
    float timer;
    public bool confused = false;

	// Use this for initialization
	void Start () {
        bloom = GetComponent<Bloom>();
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(confused)
        {
            bloom.enabled = true;
            timer += Time.deltaTime;
            bloom.bloomIntensity = Mathf.Sin(timer * 5) - 1;

        }
        else
        {
            bloom.enabled = false;
        }
	}
}
