using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndGameCanvas : MonoBehaviour
{
	Text Status;
	Image DanceImg, TombImg;
	// Use this for initialization
	void Start ()
	{
		Status = transform.FindChild ("Status").GetComponent<Text> ();
		DanceImg = transform.FindChild ("Player").GetComponent<Image> ();
		TombImg = transform.FindChild ("Tomb").GetComponent<Image> ();
		
		if (PlayerPrefs.GetInt ("Status") == 0) {
			Status.text = "YOU WIN!";
			DanceImg.gameObject.SetActive (true);
			TombImg.gameObject.SetActive (false);
		} else {
			Status.text = "YOU LOSE!";
			DanceImg.gameObject.SetActive (false);
			TombImg.gameObject.SetActive (true);
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	
	public void Menu ()
	{
		Application.LoadLevel ("Menu");
	}
}
