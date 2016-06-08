using UnityEngine;
using System.Collections;

public class CrossLineScript : MonoBehaviour
{
	public SpawnController Spawn;

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
			Spawn.StartSpawn = true;
		} 
	}
	
	
	void OnTriggerExit2D (Collider2D other)
	{
		Debug.Log (other.tag);
		if (other.tag.Equals ("Soldier")) {
			Spawn.StartSpawn = false;
		} 
	}
}
