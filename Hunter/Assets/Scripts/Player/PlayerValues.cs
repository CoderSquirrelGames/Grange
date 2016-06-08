using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerValues : MonoBehaviour
{
	public bool HasShield = false, Dead = false;
	int Life, Bullets, Honors, Points, Bombs = 0;
	List<string> Tags = new List<string> ();
	int More10 = 0, More100 ;
	List<string> EnemyTags = new List<string> ();
	public bool HaveBullets { get { return Bullets > 0; } }
	public bool HaveBombs { get { return Bombs > 0; } }
	// Use this for initialization
	PlayerControlls Controlls;
	int ShieldTime = 0;

	
	void Start ()
	{
		Controlls = GetComponent<PlayerControlls> ();
		Tags.Add ("Honor");
		Tags.Add ("UpLife");
		Tags.Add ("UpBullet");
		Tags.Add ("UpTime");
		Tags.Add ("UpGuns");
		Tags.Add ("UpShield");
		EnemyTags.Add ("Robot");
		
		Life = 10;
		
		Bullets = 80;
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckDeath ();
		CheckBombs ();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
	
		if (other.tag.Equals ("Stairs")) {
			FinalGame ();
		}
		/*
		if (other.tag.Equals ("WoodBox")) {
			other.GetComponent<WoodBoxScript> ().DealShoot ();
		} else 
		*/
		if (other.tag.Equals ("Guard") && !HasShield) {
			Debug.Log (other.tag);
			Life = 0;
			CheckDeath ();
		} else if (other.tag.Equals ("Robot")) {
			SetDamage (3);
		} else if (Tags.Contains (other.tag)) {
			SetPoints (5);
			switch (other.tag) {
			case "Honor":
				Honors++;
				More10++;
				if (More10 == 10) {
					More10 = 0;
					Bullets += 10;
				}
				break;
			case "UpLife":
				Life++;
				break;
			case "UpBullet":
				Bullets++;
				break;
			case "UpGuns":
				
				StartCoroutine (TimeWithGuns ());
				Bullets++;
				break;
			case "UpShield":
			
				HasShield = true;
				ShieldTime += 60;
				Controlls.GetGame ().UseShield ();
				//Controlls.AcquireShield ();
				StartCoroutine (TimeWithShield ());
				break;
			}
			other.GetComponent<ColectedItensScript> ().DestroyMe ();
			
		} 
		
		
	}
	
	void CheckDeath ()
	{
		if (Life <= 0) {
			if (!Dead) {
				Dead = true;
				GetComponent<AudioSource> ().Play ();
				StartCoroutine (EndGame ());
			}
			
			
		}
	}
	
	public void FinalGame ()
	{
		PlayerPrefs.SetInt ("Status", 0);
		Application.LoadLevel ("EndGame");
	}
	void CheckBombs ()
	{
		
		Controlls.GetGame ().Bomb (Bombs > 0);
	}
	
	IEnumerator EndGame ()
	{
		GetComponent<Animator> ().SetTrigger ("Dead");
		yield return new WaitForSeconds (2);
		PlayerPrefs.SetInt ("Status", 1);
		Application.LoadLevel ("EndGame");
	
	}
	IEnumerator TimeWithGuns ()
	{
		GetComponent<Animator> ().SetInteger ("WeaponType", 1);
		yield return new WaitForSeconds (10);
		GetComponent<Animator> ().SetInteger ("WeaponType", 0);
	}
	IEnumerator TimeWithShield ()
	{
		yield return new WaitForSeconds (1);
		if (ShieldTime > 1) {
			Controlls.GetGame ().SetTime (ShieldTime);
			StartCoroutine (TimeWithShield ());
			ShieldTime--;
		} else {
			Controlls.GetGame ().NotUseShield ();
			HasShield = false;
		}
		
	}
	
	
	public int GetLife ()
	{
		return Life;
	}
	
	public int GetBullets ()
	{
		return Bullets;
	}
	
	public int GetHonors ()
	{
		return Honors;
	}
	public void Shoot ()
	{
		Bullets--;
	}
	public void SetDamage (int amout)
	{
//		GetComponent<AudioSource> ().Play ();
		Life -= amout;
	}
	
	public int GetPoints ()
	{
		return Points;
	}
	public void SetPoints (int amout)
	{
		Points += amout;
		More100 += amout;
		if (More100 >= 100) {
			More100 = More100 - 100;
			Bombs++;
			
		}
	}
	public void ThrowBomb ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector3 BombPosition = ray.origin;
		GameObject newBomb = Instantiate (Controlls.Bomb, transform.position, transform.rotation) as GameObject;
		newBomb.transform.parent = Controlls.InstantsPlace.transform;
		newBomb.GetComponent<BombScript> ().SetEndPosition (BombPosition);
		
		Bombs--;
	}
}
