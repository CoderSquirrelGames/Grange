using UnityEngine;
using System.Collections;

public class MenuCanvasController : MonoBehaviour
{
	GameObject CreditsPanel, ControlsPanel, HelpPanel;

	// Use this for initialization
	void Start ()
	{
		ControlsPanel = transform.FindChild ("ControlsPanel").gameObject;
		CreditsPanel = transform.FindChild ("CreditsPanel").gameObject;
		HelpPanel = transform.FindChild ("HelpPanel").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	
	public void Play ()
	{
		Application.LoadLevel ("Game");
	}
	
	public void Credits ()
	{
		CreditsPanel.SetActive (true);
	}

	public void Back ()
	{
		CreditsPanel.SetActive (false);
		HelpPanel.SetActive (false);
		ControlsPanel.SetActive (false);
	}
	public void ShowsHelp ()
	{
		HelpPanel.SetActive (true);
	}
	public void Controls ()
	{
		ControlsPanel.SetActive (true);
	}
	
	public void Exit ()
	{
		Application.Quit ();
	}
}
