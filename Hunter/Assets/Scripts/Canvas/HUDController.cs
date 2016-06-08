using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class HUDController : MonoBehaviour
{

	public Text Life { get; set; }
	public Text Bullets{ get; set; } 
	public Text Honor { get; set; }
	public Text Points { get; set; }
	public Image Weapon;
	public List<Sprite>Imgs;
	// Use this for initialization
	void Start ()
	{
		Life = transform.FindChild ("LifeText").GetComponent<Text> ();
		Bullets = transform.FindChild ("BulletsText").GetComponent<Text> ();
		Honor = transform.FindChild ("HonorText").GetComponent<Text> ();
		Points = transform.FindChild ("PointsText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	
	
}
