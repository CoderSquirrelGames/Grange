using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BombScript : MonoBehaviour
{

	Vector3 StartPos, EndPosition;
	float Scale;
	List<string> TAGS = new List<string> ();
	// Use this for initialization
	
		
	// Use this for initialization
	void Start ()
	{
		TAGS.Add ("Soldier");
		TAGS.Add ("SoldierBullet");
		TAGS.Add ("Player");
		TAGS.Add ("Door");
		TAGS.Add ("UpLife");
		TAGS.Add ("UpBullet");
		TAGS.Add ("UpGuns");
		TAGS.Add ("Honor");
		TAGS.Add ("Sight");
		TAGS.Add ("ForceField");
		Scale = transform.localScale.x;
		StartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (transform.position != EndPosition) {
			
			Vector3 separatation = EndPosition - transform.position;
			float step = 3f * Time.deltaTime;	
			if (separatation.magnitude < step) {
				transform.position = EndPosition;
				return;
			}			
			Vector3 direction = separatation;
			direction *= step;
			transform.position = transform.position + direction;
		} else {
			ExplodeAnimation ();
		}
		
		/*
		if (Vector2.Distance (transform.position, EndPosition) < 1f) {
			
		} else {

			if (Vector3.Distance (StartPos, transform.position) <= Vector3.Distance (transform.position, EndPosition) / 2) {
				transform.localScale += new Vector3 (Scale * 0.10f, Scale * 0.10f, Scale * 0.10f);
			} else {
				transform.localScale -= new Vector3 (Scale * 0.10f, Scale * 0.10f, Scale * 0.10f);
			}
		}
		*/
	}
	
	
	void ExplodeAnimation ()
	{
		GetComponent<Animator> ().enabled = true;
		GetComponent<PolygonCollider2D> ().enabled = true;
		Destroy (gameObject, 1);
	}
	
	
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (!TAGS.Contains (other.tag)) {
			Debug.Log (other.tag);
			if (other.tag.Equals ("Guard")) {
				other.GetComponent<GuardStcript> ().Die ();
			}
		}
	}
	
	public void SetEndPosition (Vector3 endPos)
	{
		EndPosition = endPos;
		
	}
}
