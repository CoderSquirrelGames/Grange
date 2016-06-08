using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CanvasController : MonoBehaviour
{
	GameController GAME;
	Text TimeTxt;
	public GameObject MobileControls;
	// Use this for initialization
	void Start ()
	{
		transform.FindChild ("ButtonBackground").FindChild ("ButtonBomb").GetComponent<Image> ().color = Color.gray;
		
		transform.FindChild ("ButtonBackground").FindChild ("ButtonShield").GetComponent<Image> ().color = Color.gray;
		GAME = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		TimeTxt = transform.FindChild ("ButtonBackground").FindChild ("ButtonShield").FindChild ("Time").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Shield ()
	{
		GAME.Shield ();
	}
	
	public void SetMobileControlsOn ()
	{
		MobileControls.SetActive (true);
	}
	
	public void ChangeWeapon ()
	{
		GAME.ChangeWeapon ();
	}
	
	public void Fire ()
	{
		GAME.Fire ();
	}
	
	public void ShieldButton (bool state)
	{
		if (state)
			transform.FindChild ("ButtonBackground").FindChild ("ButtonShield").GetComponent<Image> ().color = Color.white;
		else
			transform.FindChild ("ButtonBackground").FindChild ("ButtonShield").GetComponent<Image> ().color = Color.gray;
	}
	
	public void SetTime (int time)
	{
		TimeTxt.text = time.ToString ();
	}
	
	public void Bomb (bool has)
	{
	
		if (has) {
			transform.FindChild ("ButtonBackground").FindChild ("ButtonBomb").GetComponent<Image> ().color = Color.white;
		} else {
			transform.FindChild ("ButtonBackground").FindChild ("ButtonBomb").GetComponent<Image> ().color = Color.gray;
		}
	}
	
}
