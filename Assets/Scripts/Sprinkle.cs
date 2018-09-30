using UnityEngine;
using System.Collections;

public class Sprinkle : MonoBehaviour {

	public Color[] colors = {Color.white, Color.red, Color.blue, Color.yellow};
	public GameObject playerController;
    public GameObject SprinkleExplosion;

	// Use this for initialization
	void Start () {
		Renderer rend = gameObject.GetComponent<Renderer> ();

		// switch from white to any of the colors
		rend.material.color = colors [Random.Range (0, colors.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag != "Details" && collision.gameObject.tag != "Enemy") {
			// 0-10 damage, range of a few m
			float damage = Mathf.SmoothStep(0f, 10f, 
				(10.0f - Vector3.Distance (transform.position, playerController.transform.position)) / 10.0f);

			playerController.SendMessage ("Damage", damage);

            // play particle effect
            SprinkleExplosion.SetActive(true);
            Instantiate(SprinkleExplosion, transform.position, transform.rotation);
			Destroy (gameObject);
            SprinkleExplosion.SetActive(false);
        }
	}
}
