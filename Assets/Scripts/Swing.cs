using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void startSwing()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            GetComponent<Animator>().Play("Forehand", -1, 0f);
        }
        else if (rand == 1)
        {
            GetComponent<Animator>().Play("Backhand 0", -1, 0f);
        }
    }
}
