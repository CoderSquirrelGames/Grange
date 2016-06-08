using UnityEngine;
using System.Collections;

public class SightScript : MonoBehaviour
{
	RobotController Controller;

	// Use this for initialization
	void Start ()
	{
		Controller = transform.parent.GetComponent<RobotController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag.Equals ("Soldier")) {
			Controller.StartShoot ();

			
		}
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		
		if (other.tag.Equals ("Soldier")) {
			Controller.StopShoot ();
			
			
		}
	}
}
