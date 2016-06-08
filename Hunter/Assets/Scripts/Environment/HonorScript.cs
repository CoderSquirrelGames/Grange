using UnityEngine;
using System.Collections;

public class HonorScript : MonoBehaviour
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
	
	public void DestroyMe ()
	{
		GAME.AddPlayersPoints (2);
		Destroy (gameObject);
	}
}
