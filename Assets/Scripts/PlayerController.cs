using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float xSpeed, movSpeed;
    Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody> ();
        rb.velocity =  new Vector3 (0f, 0.0f, movSpeed);
    }
    void FixedUpdate(){
        rb.velocity =  new Vector3 (0f, rb.velocity.y, movSpeed);
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
        //if WINDOWS
        // float horizontalAxis = Input.GetAxis ("Horizontal");
        // bool buttonHorizontal = Input.GetButtonDown ("Horizontal");
        //If Android-iPhone
        if(Time.time > delayBetweenMovements){
			float horizontalAxis = Input.acceleration.x;
			bool buttonHorizontal = Mathf.Abs (horizontalAxis) > hAccValue;
			if (buttonHorizontal) {
				print ("- Horizontal Axis " + horizontalAxis);
//                if(Mathf.Abs(horizontalAxis) > 1){
//                    horizontalAxis += horizontalAxis > 0 ? -1:1;
//				}
//				print ("- - " + horizontalAxis);
                
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
