using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public PlayerControlls Player;
	public CanvasController CANVAS;
	
	// Use this for initialization
	void Start ()
	{
		//Player = GameObject.FindGameObjectWithTag ("Soldier").GetComponent<PlayerControlls> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Bomb (bool state)
	{
		CANVAS.Bomb (state);
	}
	
	public void Shield ()
	{
		Player.Shield ();
	}

	
	public void ChangeWeapon ()
	{
		Player.ChangeWeapon ();
	}
	
	public void Fire ()
	{
		Player.Attack ();
	}
	
	
	
	public void UseShield ()
	{
		CANVAS.ShieldButton (true);
	}
	public void NotUseShield ()
	{
		CANVAS.ShieldButton (false);
	}
	
	
	public void SetTime (int time)
	{
		CANVAS.SetTime (time);
	}
	
	
	
	
	public void AddPlayersPoints (int amount)
	{
		Player.AddPoints (amount);
	}
	
	
}
