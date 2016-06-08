
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
	GameController GAME;
	
	public List<GameObject>SpawnPoints;
	public bool KeepSpawning = true, StartSpawn = false;
	int Life = 10;
	public GameObject Robot	;// Use this for initialization
	void Start ()
	{
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (StartSpawn) {
			StartSpawn = false;
			StartCoroutine (Spawn ());
		}
	}
	
	IEnumerator Spawn ()
	{
		yield return new WaitForSeconds (Random.Range (1, 15));
		for (int i = 0; i < Random.Range(1, 4); i++) {
		
			GameObject go = Instantiate (Robot, SpawnPoints [Random.Range (0, SpawnPoints.Count)].transform.position, transform.rotation) as GameObject;
			go.transform.SetParent (transform);
		}
		if (KeepSpawning)
			StartCoroutine (Spawn ());
	}
	
	public void Damage ()
	{
		Life--;
		if (Life < 1) {
			KeepSpawning = false;
			GAME.AddPlayersPoints (40);
		}
	}
	
}
