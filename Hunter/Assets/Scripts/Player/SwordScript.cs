using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour
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

		if (other.tag.Equals ("WoodBox")) {
			other.GetComponent<WoodBoxScript> ().DealShoot ();
		} else if (other.tag.Equals ("Robot")) {
			other.GetComponent<RobotController> ().Damage ();
		} else if (other.tag.Equals ("RobotBuilder")) {
			other.GetComponent<SpawnController> ().Damage ();
		}	
	}
	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag.Equals ("Robot")) {
			other.GetComponent<RobotController> ().Damage ();
		}
	}
	
	
}
