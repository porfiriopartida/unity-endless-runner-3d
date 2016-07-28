using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {
	public bool right;
	GameObject piecesWrapper;
	PlayerController player;
	// Use this for initialization
	void Start () {
		piecesWrapper = GameObject.Find ("Pieces");
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}
	void OnTriggerEnter(Collider collider){
//		Transform transform = piecesWrapper.transform;
		//		transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), -90);
//		collider.transform.RotateAround (collider.transform.position, new Vector3(0, 1, 0), 90);
        if (right) {
            player.RotateRight ();
        } else {
			player.RotateLeft ();
        }
        gameObject.SetActive (false);
//		transform.position = transform.position + new Vector3(10, 0, 0);
	}
}
