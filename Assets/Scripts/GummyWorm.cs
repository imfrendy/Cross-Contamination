using UnityEngine;
using System.Collections;

public class GummyWorm : MonoBehaviour {
	public GameObject playerController;
	Rigidbody body;

	public float maxHealth = 1000f;
	float health;

	public float RotationSpeed;

	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;


	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody> ();
		//body.MoveRotation (Quaternion.Euler (new Vector3 (-90, 0, 0)));

	}

	// Update is called once per frame
	void Update()
	{
		//find the vector pointing from our position to the target
		_direction = (playerController.transform.position - transform.position).normalized;

		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);

		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
	}

	void OnCollisionEnter (Collision collision)
	{

	}
}
