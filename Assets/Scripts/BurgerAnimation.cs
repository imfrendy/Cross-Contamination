using UnityEngine;
using System.Collections;

public class BurgerAnimation : MonoBehaviour {

	void Jump ()
	{
		GetComponent<Animator> ().Play ("Full", -1, 0f);
	}
}
