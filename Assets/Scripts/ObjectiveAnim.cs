using UnityEngine;
using System.Collections;

public class ObjectiveAnim : MonoBehaviour {
    void Floating ()
    {
        GetComponent<Animator>().Play("Floating", -1, 0f);
    }

	void PleaseDie()
    {
        GetComponent<Animator>().Play("Die", -1, 0f);
    }
}
