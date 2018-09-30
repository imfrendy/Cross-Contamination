using UnityEngine;
using System.Collections;

public class BreadAnimation : MonoBehaviour {
	void Crawl ()
	{
		GetComponent<Animator> ().Play ("Crawl 0", -1, 0f);
	}

	void Jump ()
	{
		GetComponent<Animator> ().Play ("Jump", -1, 0f);
	}
}
