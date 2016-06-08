using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BulletMove : MonoBehaviour
{
	float speed = 10f;
	
	public ParticleSystem PS;
	bool Move = true;
	List<string> TAGS = new List<string> ();
	// Use this for initialization
	void Start ()
	{
		TAGS.Add ("Soldier");
		TAGS.Add ("SoldierBullet");
		TAGS.Add ("Player");
		TAGS.Add ("Door");
		TAGS.Add ("UpLife");
		TAGS.Add ("UpBullet");
		TAGS.Add ("UpGuns");
		TAGS.Add ("Honor");
		TAGS.Add ("Sight");
		TAGS.Add ("ForceField");
		TAGS.Add ("Room");
		//	PS = transform.FindChild ("PS") as ParticleSystem;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Move) {
			/*Vector3 pos = transform.position;
			Vector3 velocity = new Vector3 (PosX, speed * Time.deltaTime, 0);
			pos += transform.rotation * velocity;
			pos.x = PosX;
			transform.position = pos;*/
			
			
			
			
			Vector3 pos = transform.position;
			Vector3 velocity = new Vector3 (0, speed * Time.deltaTime, 0);
			pos += transform.rotation * velocity;

			transform.position = pos;
		} 
	}
	
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (!TAGS.Contains (other.tag)) {
			Move = false;
			PS.Play ();
			if (other.tag.Equals ("WoodBox")) {
				other.GetComponent<WoodBoxScript> ().DealShoot ();
			} else if (other.tag.Equals ("Robot")) {
				other.GetComponent<RobotController> ().Damage ();
			} else if (other.tag.Equals ("RobotBuilder")) {
				other.GetComponent<SpawnController> ().Damage ();
			}	
		
			Destroy (gameObject);
		}
	
	}
}
