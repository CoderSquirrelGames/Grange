using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform Soldier;
	float yOffset = 5;
	// Use this for initialization
	void Start ()
	{
		//Soldier = GameObject.FindGameObjectWithTag ("Soldier").transform; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 (Soldier.position.x, Soldier.position.y + yOffset, -20); 
	}
}
