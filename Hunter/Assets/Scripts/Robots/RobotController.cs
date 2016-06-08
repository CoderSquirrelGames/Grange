using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotController : MonoBehaviour
{
	GameController GAME;
	Animator anim;  
	float speed = 4f, zAngle;
	Vector3 dir, pos, velocity;
//	float StartY, StartX;
	int Life = 2;
	bool Move = true;
	public ParticleSystem PS;
	List<string> Tags = new List<string> ();
	Transform Player;
	
	public GameObject Bullet, Firepoint;
	// Use this for initialization
	void Start ()
	{
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
		Player = GameObject.Find ("Soldier").transform;
		
		Tags.Add ("Blade");
		Tags.Add ("SoldierBullet");
		
		//StartY = transform.position.y;
		//StartX = transform.position.x;
		speed = Random.Range (2, 4);
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<Animator> ().SetBool ("Walk", Move);
		if (Move) {
			
			/*
			Vector3 pos = transform.position;
			Vector3 velocity = new Vector3 (0, speed * Time.deltaTime, 0);
			pos -= transform.rotation * velocity;
			if (MoveOnY)
				pos.x = StartX;
			else	
				pos.y = StartY;
			transform.position = pos;
		*/
			dir = Player.position - transform.position;
			
			dir.Normalize ();
			
			zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
			
			
			transform.rotation = Quaternion.Euler (0, 0, zAngle);
			
			pos = transform.position;
			velocity = new Vector3 (0, speed * Time.deltaTime, 0);
			pos += transform.rotation * velocity;
			
			transform.position = pos;
		}
		
		
		if (GetComponent<Animator> ().GetBool ("Shoot")) {
			
			
			
			Vector3 dir = Player.transform.position - transform.position;
			
			dir.Normalize ();
			
			float zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 90;
			
			
			transform.rotation = Quaternion.Euler (0, 0, zAngle);
			
			
		
		}
	}
	/*
	void OnTriggerEnter2D (Collider2D other)
	{

		if (Tags.Contains (other.tag)) {
			Life--;
			CheckDead ();
		}
		
	}
	
	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag.Equals ("Blade")) {
			
			
		}
	}
	*/
	
	
	void CheckDead ()
	{
		if (Life < 1) {
			GAME.AddPlayersPoints (10);
			Move = false;
			GetComponent<Animator> ().SetBool ("IsDead", true);
			PS.Play ();
			transform.FindChild ("Sight").transform.gameObject.SetActive (false);
			//Destroy (gameObject, 2);
			GetComponent<PolygonCollider2D> ().enabled = false;
			this.enabled = false;
			GetComponent<Rigidbody2D> ().isKinematic = false;
			
		}
	}
	
	
	public void Damage ()
	{
		Life--;
		CheckDead ();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag.Equals ("Corner")) {
			transform.rotation = Quaternion.identity;
			//MoveOnY = true;
			//	StartX = transform.position.x;
		} else if (other.tag.Equals ("Start")) {
			Destroy (gameObject);
		} else if (other.tag.Equals ("Soldier")) {
			Move = false;
			
		} 
	}
	

	void OnTriggerExit2D (Collider2D other)
	{
		Debug.Log (other.tag);
	
		if (other.tag.Equals ("Soldier")) {
			Move = true;
			
		} else if (other.tag.Equals ("Room")) {
			Destroy (gameObject);
			
		} 
	}

	public void StartShoot ()
	{
		GetComponent<Animator> ().SetBool ("Shoot", true);
		GetComponent<Animator> ().SetBool ("Walk", false);
		
		
		

	}
	
	public void Shoot ()
	{
		GameObject newBullet = Instantiate (Bullet, Firepoint.transform.position, transform.rotation) as GameObject;
		newBullet.gameObject.transform.SetParent (transform);
	}
	
	public void StopShoot ()
	{
		
		GetComponent<Animator> ().SetBool ("Shoot", false);
		GetComponent<Animator> ().SetBool ("Walk", true);
	}
	
	

}

