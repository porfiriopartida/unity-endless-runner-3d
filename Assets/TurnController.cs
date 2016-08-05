using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {
	public bool right;
	GameObject piecesWrapper;
	PlayerController playerController;
	// Use this for initialization
	void Start () {
		piecesWrapper = GameObject.Find ("Pieces");
		playerController = GameObject.Find ("Player").GetComponent<PlayerController>();
	}
	void OnTriggerEnter(Collider collider){
//		Transform transform = piecesWrapper.transform;
		//		transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), -90);
//		collider.transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), 90);
		if(collider.transform.tag == "Player"){
	        if (right) {
	            playerController.RotateRight ();
				DebugScript.self.addText("ROTATE_RIGHT");
	        } else {
				playerController.RotateLeft ();
				DebugScript.self.addText("ROTATE_LEFT");
			}
			playerController.recenter ();

			//Destroy the turn box as soon as you turn
			gameObject.SetActive (false);
		}
	}
}
