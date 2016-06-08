using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WoodBoxScript : MonoBehaviour
{
	GameController GAME;
	public List<GameObject> Pups;
	bool NotShooted = true;
	int Shoots = 2;
	int PupIndex;
	// Use this for initialization
	void Start ()
	{
		PupIndex = Random.Range (0, Pups.Count);
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
		GetComponent<Animator> ().SetBool ("Shooted", false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Shoots < 1 && NotShooted == true) {
			NotShooted = true;
			Brooke ();
		}
	}
	
	public void DealShoot ()
	{
		GAME.AddPlayersPoints (1);
		Shoots--;
	}
	
	
	void Brooke ()
	{
		GetComponent<AudioSource> ().Play ();
		NotShooted = false;
		GetComponent<Animator> ().SetBool ("Shooted", true);
		GetComponent<BoxCollider2D> ().enabled = false;
		Pups [PupIndex].SetActive (true);
	}
}
