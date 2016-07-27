using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {
	public bool right;
	GameObject piecesWrapper;
	// Use this for initialization
	void Start () {
		piecesWrapper = GameObject.Find ("Pieces");
	}
	void OnTriggerEnter(Collider collider){
//		Transform transform = piecesWrapper.transform;
		//		transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), -90);
//		collider.transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), 90);
        if (right) {
            PlayerController.rotateRight ();
        } else {
            PlayerController.rotateLeft ();
        }
        gameObject.SetActive (false);
//		transform.position = transform.position + new Vector3(10, 0, 0);
	}
}
