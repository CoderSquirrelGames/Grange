using UnityEngine;
using System.Collections;

public class GuardStcript : MonoBehaviour
{
	GameController GAME;
	// Use this for initialization
	void Start ()
	{
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Die ()
	{
		GAME.AddPlayersPoints (20);
		GetComponent<PolygonCollider2D> ().enabled = false;
		GetComponent<Animator> ().SetTrigger ("Die");
	}
}
