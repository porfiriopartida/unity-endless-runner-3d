using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float xSpee;
	public static float movSpeed = 30f;
    Rigidbody rb;
	public static Vector3 velocity = new Vector3 (0f, 0.0f, movSpeed);
	public static void rotateRight(){
		GameObject player = GameObject.Find ("Player");
		velocity = new Vector3 (movSpeed, 0.0f, 0f);
		player.transform.RotateAround(player.transform.position, new Vector3(0, 1, 0), 90);
	}
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody> ();
		rb.velocity =  velocity;
	}
	void FixedUpdate(){
		rb.velocity =  velocity;
    }
    float currentPosition = 0;

    float hAccValue = 0.3f; //if windows = 0.
    enum Movements { LEFT, RIGHT, NONE};
    private Movements lastMovement = Movements.NONE;
    float delayRepeatedMovements;
    float delayBetweenMovements;
    float timeBetweenMovements = 0.5f;
    float timeBetweenRepeatedMovements = 1f;
    // Update is called once per frame
    void Update () {
		if(Time.time > delayBetweenMovements){
			//if WINDOWS
//			float horizontalAxis = Input.GetAxis ("Horizontal");
//			bool buttonHorizontal = Input.GetButtonDown ("Horizontal");
			//If Android-iPhone
			float horizontalAxis = Input.acceleration.x;
			bool buttonHorizontal = Mathf.Abs (horizontalAxis) > hAccValue;
			if (buttonHorizontal) {
				print ("- Horizontal Axis " + horizontalAxis);
				Movements actualMovement = Movements.NONE;
				float diff = 0;
				if (horizontalAxis < hAccValue && currentPosition >= 0) {
                    //To the left
                    actualMovement = Movements.LEFT;
                    diff = -10f;
                } else if (horizontalAxis > hAccValue && currentPosition <= 0) {
                    //To the right
                    actualMovement = Movements.RIGHT;
                    diff = +10f;
                }
                //Using delay
                if(actualMovement != lastMovement || Time.time >= delayRepeatedMovements && actualMovement == lastMovement){
                    currentPosition += diff;
					transform.Translate(currentPosition * Time.smoothDeltaTime, 0, 0);
                    transform.position = new Vector3 (currentPosition, transform.position.y, transform.position.z);
                    delayRepeatedMovements = Time.time + timeBetweenRepeatedMovements;
                    delayBetweenMovements = Time.time + timeBetweenMovements;
                    lastMovement = actualMovement;
                }
            } else {
                delayRepeatedMovements = 0;
            }
        }
    }
}
