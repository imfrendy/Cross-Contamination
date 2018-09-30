using UnityEngine;
using System.Collections;

public class BarrettAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void ReloadAnim() {
        GetComponent<Animator>().Play("reload", -1, 0f);
    }

    void Scope ()
    {
        GetComponent<Animator>().Play("scope", -1, 0f);
    }

    void Unscope()
    {
        GetComponent<Animator>().Play("unscope", -1, 0f);
    }

    void Fire ()
    {
        GetComponent<Animator>().Play("Fire", -1, 0f);
    }
}
