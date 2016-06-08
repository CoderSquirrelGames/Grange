using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	Transform Player;
	float speed = 10f, zAngle;
	Vector3 dir, pos, velocity;
	// Use this for initialization
	void Start ()
	{
		Player = GameObject.Find ("Soldier").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		dir = Player.position - transform.position;
	
		dir.Normalize ();
	
		zAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
		
	
		transform.rotation = Quaternion.Euler (0, 0, zAngle);

		pos = transform.position;
		velocity = new Vector3 (0, speed * Time.deltaTime, 0);
		pos += transform.rotation * velocity;
		
		transform.position = pos;
	}
	
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag.Equals ("ForceField")) {
			Destroy (gameObject);
		} else if (other.tag.Equals ("Soldier")) {
			other.GetComponent<PlayerValues> ().SetDamage (1);
			Destroy (gameObject);
		}
	}
}