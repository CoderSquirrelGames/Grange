using UnityEngine;
using System.Collections;

public class ColectedItensScript : MonoBehaviour
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
		GAME.AddPlayersPoints (5);
		GetComponent<AudioSource> ().Play ();
		GetComponent<PolygonCollider2D> ().enabled = false;
		Destroy (gameObject, 0.1f);
	}
}
