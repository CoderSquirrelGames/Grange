using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControlls : MonoBehaviour
{
	public GameObject InstantsPlace, Bomb;
	public List<GameObject> FirePoints, Bullets;
	public float RotVel = 5, MoveVel = 5, Offset;
	public HUDController HUD;
	public bool IsMobile;
	GameController GAME;
	Animator anim;  
	Vector3 pos ;
	PlayerValues Values;
	bool HaveTwoWeapons, ThrowingBomb;
	GameObject ShieldObj;
	Vector3 BombPosition;
	Ray ray ;
	RaycastHit2D hit;
	
	void Start ()
	{
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		ShieldObj = transform.FindChild ("ForceField").gameObject;
		//GetComponent<Animator> ().SetBool ("Weapon", true);
		anim = GetComponent<Animator> ();
		anim.SetInteger ("WeaponType", 0);
		Values = GetComponent<PlayerValues> ();
		anim = GetComponent<Animator> ();
		
			
	 	
	}
	
	// Update is called once per frame
	void Update ()
	{

		
		if (!Values.Dead) {
			MovePlayer ();
			RotatePlayer ();
			HUD.Weapon.sprite = HUD.Imgs [anim.GetInteger ("WeaponType")];
			if (Input.GetMouseButtonDown (0) && !ThrowingBomb) {
				Attack ();
			} else if (Input.GetMouseButtonDown (0) && ThrowingBomb) {
				if (Values.HaveBombs) {
					Values.ThrowBomb ();
				}
			} else if (Input.GetMouseButtonDown (1)) {
		
				Shield ();
			
			}
			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
				ChangeWeapon ();
			} else if (Input.GetKeyDown (KeyCode.LeftControl) || Input.GetKeyDown (KeyCode.RightControl)) {
				ThrowingBomb = !ThrowingBomb;
			}
			
			if (Input.GetKeyDown (KeyCode.Z)) {

			
			} else if (Input.GetKeyDown (KeyCode.C)) {
				Attack ();
			} 
		
		}
			
		
	
		HUD.Bullets.text = Values.GetBullets ().ToString ();
		HUD.Life.text = Values.GetLife ().ToString ();
		HUD.Honor.text = Values.GetHonors ().ToString ();
		HUD.Points.text = Values.GetPoints ().ToString ();
		
	}


	
	/// <summary>
	/// Moves the player usign keyboard
	/// </summary>
	public void MovePlayer ()
	{
		if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0) {
			//MoveVel = 0;
			anim.SetBool ("Walk", false);
			return;
		} 
		anim.SetBool ("Walk", true);


		
		Vector3 pos = transform.position;
		Vector3 velocity = new Vector3 (0, Input.GetAxis ("Vertical") * MoveVel * Time.deltaTime, 0);
		pos.y += Input.GetAxis ("Vertical") * MoveVel * Time.deltaTime;
		pos.x += Input.GetAxis ("Horizontal") * MoveVel * Time.deltaTime;
		pos += velocity;
		
		transform.position = pos;
	}
	
	
	
	void RotatePlayer ()
	{/*
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 dir = Input.mousePosition;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		
		Vector3 difference = Camera.main.WorldToScreenPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();
		float z = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, z + Offset);
		*/
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.rotation = Quaternion.LookRotation (Vector3.forward, mousePos - transform.position);
	}
	
	public void Shooting ()
	{
		anim.SetBool ("Attack", false);		
		if (anim.GetInteger ("WeaponType") < 2) {
			if (Values.HaveBullets) {
				for (int i = 0; i <= anim.GetInteger ("WeaponType"); i++) {
					Values.Shoot ();
					if (anim.GetInteger ("WeaponType") != i) {
						GameObject newBullet = Instantiate (Bullets [anim.GetInteger ("WeaponType")], FirePoints [2].transform.position, transform.rotation) as GameObject;
						newBullet.gameObject.transform.SetParent (InstantsPlace.transform);
					} else {
						GameObject newBullet = Instantiate (Bullets [anim.GetInteger ("WeaponType")], FirePoints [i].transform.position, transform.rotation) as GameObject;
						newBullet.gameObject.transform.SetParent (InstantsPlace.transform);
					}
					
				}
			}
		} else {
			//	GetComponent<Animator> ().SetBool ("Weapon", false);
			anim.SetInteger ("WeaponType", 2);
		}
	}
	

		
	public void Attack ()
	{
		anim.SetBool ("Attack", true);
	}
	
	public void ChangeWeapon ()
	{
	
		if (anim.GetInteger ("WeaponType") == 0) {
			anim.SetInteger ("WeaponType", 2);
		} else {
			anim.SetInteger ("WeaponType", 0);
			HUD.Weapon.sprite = HUD.Imgs [anim.GetInteger ("WeaponType")];
		}
	}
/// <summary>
/// Makes the move using joystick
/// </summary>
/// <param name="zAxys">Z axys.</param>
/// <param name="yAxys">Y axys.</param>
	public void MakeMove (float zAxys, float yAxys)
	{
		anim.SetBool ("Walk", true);
		if (zAxys == 0 && yAxys == 0) {
			//MoveVel = 0;
			anim.SetBool ("Walk", false);
		} 
		Quaternion rot = transform.rotation;
		
		float z = rot.eulerAngles.z;
		
		z += zAxys * RotVel * Time.deltaTime;
		
		rot = Quaternion.Euler (0, 0, z);
		transform.rotation = rot;
		
		Vector3 pos = transform.position;
		Vector3 velocity = new Vector3 (0, yAxys * MoveVel * Time.deltaTime, 0);
		//pos.y += Input.GetAxis ("Vertical") * MoveVel * Time.deltaTime;
		
		pos += rot * velocity;
		
		transform.position = pos;
	}
	
	/*
	public void AcquireShield ()
	{
		GAME.UseShield ();
	}

	public void ReleaseShield ()
	{
		GAME.NotUseShield ();
	}*/
	public void Shield ()
	{
		if (Values.HasShield)
			ShieldObj.SetActive (!ShieldObj.activeSelf);
	}
	
	public void AddPoints (int amount)
	{
		Values.SetPoints (amount);
	}
	
	public GameController GetGame ()
	{
		return GAME;
	}
	


}
