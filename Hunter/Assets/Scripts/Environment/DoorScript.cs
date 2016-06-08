using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag.Equals ("Soldier")) {
			//transform.FindChild ("LeftDoor").GetComponent<Rigidbody2D> ().isKinematic = false;
			//GetComponent<Rigidbody2D> ().isKinematic = false;
			GetComponent<Animator> ().SetBool ("Open", true);

		}
	}
}
