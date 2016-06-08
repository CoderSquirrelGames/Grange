using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{

	public GameObject Player, Canvas;
	// Use this for initialization
	void Start ()
	{
		#if UNITY_EDITOR
		#endif
		#if UNITY_ANDROID 
		Canvas.GetComponent<CanvasController> ().SetMobileControlsOn ();
		Player.GetComponent<PlayerControlls> ().IsMobile = true;
		#endif
		
		#if UNITY_IPHONE
		#endif
		
		#if UNITY_STANDALONE_OSX
		Player.GetComponent<PlayerControlls> ().IsAndroid = false;
		#endif
		
		#if UNITY_STANDALONE_WIN
		Player.GetComponent<PlayerControlls> ().IsMobile = false;
		#endif
		
		
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
