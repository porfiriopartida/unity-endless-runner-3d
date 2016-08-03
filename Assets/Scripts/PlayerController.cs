using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float m_movSpeed;
    Rigidbody rb;
    private Vector3 m_velocity;
    
    float currentPosition = 0;
    
    float hAccValue = 0.3f; //if windows = 0.
    enum Movements { LEFT, RIGHT, NONE};
    private Movements lastMovement = Movements.NONE;
    float delayRepeatedMovements;
    float delayBetweenMovements;
    float timeBetweenMovements = 0.5f;
    float timeBetweenRepeatedMovements = 1f;

    public void RotateRight(){
        rotate (true);
    }
    public void RotateLeft(){
        rotate (false);
    }
    private void rotate(bool right){
        GameObject player = GameObject.Find ("Player");
        GameObject camera = GameObject.Find ("Main Camera");
        FollowCameraController cameraController = camera.GetComponent<FollowCameraController> ();

        //velocity = new Vector3 (movSpeed, 0.0f, 0f);
		if (right) {
            if(m_velocity.x > 0){
                //Traveling through +X. Turning to the Right. -Z
                m_velocity = new Vector3 (0f, 0f, -m_movSpeed);
            } else if (m_velocity.z > 0){
                //Traveling through +Z. Turning to the Right. +X
                m_velocity = new Vector3 (+m_movSpeed, 0f, 0f);
            } else if(m_velocity.x < 0){
                //Traveling through -X. Turning to the Right. +Z
                m_velocity = new Vector3 (0f, 0f, m_movSpeed);
            }else if(m_velocity.z < 0){
                //Traveling through -X. Turning to the Right. +Z
                m_velocity = new Vector3 (-m_movSpeed, 0f, 0f);
            }
            //TODO: Travel through Y ?
		} else {
            if(m_velocity.x > 0){
                //Traveling through +X. Turning to the LEFT. -Z
                m_velocity = new Vector3 (0f, 0f, +m_movSpeed);
            } else if (m_velocity.z > 0){
                //Traveling through +Z. Turning to the LEFT. +X
                m_velocity = new Vector3 (-m_movSpeed, 0f, 0f);
            } else if(m_velocity.x < 0){
                //Traveling through -X. Turning to the LEFT. +Z
                m_velocity = new Vector3 (0f, 0f, -m_movSpeed);
            }else if(m_velocity.z < 0){
                //Traveling through -X. Turning to the LEFT. +Z
                m_velocity = new Vector3 (+m_movSpeed, 0f, 0f);
            }
            //TODO: Travel through Y ?
        }

        float angle = (right ? 90 : -90);
        //player.transform.RotateAround(player.transform.position, new Vector3(0, 1, 0), angle);
        cameraController.buildOffset(m_velocity);
        player.transform.Rotate (new Vector3(0, angle, 0));
        rb.velocity =  new Vector3(0,0,0);
        rb.AddForce(m_velocity);
    }
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody> ();
        m_velocity = new Vector3 (0f, 0.0f, m_movSpeed);
        rb.AddForce(m_velocity);
    }
	void OnCollisionEnter(Collision collision){
		DebugScript.self.addText("COLLISION: " + collision.transform.tag);
		if(collision.transform.tag == "PIECE"){
			Collider collider = collision.transform.GetComponent<Collider>();
			float x = collider.bounds.size.x;
			float y = collider.bounds.size.y;
			DebugScript.self.addText("Size: ("+x+", "+y+")");
		}
	}
//    void FixedUpdate(){
//        rb.velocity =  velocity;
//    }
    // Update is called once per frame
    void Update () {
        if(Time.time > delayBetweenMovements){
            //if WINDOWS
            //If Android-iPhone
			#if UNITY_EDITOR
            float horizontalAxis = Input.GetAxis ("Horizontal");
            bool buttonHorizontal = Input.GetButtonDown ("Horizontal");
			hAccValue = 0;
			#else
            float horizontalAxis = Input.acceleration.x;
            bool buttonHorizontal = Mathf.Abs (horizontalAxis) > hAccValue;
			#endif
			if (buttonHorizontal) {
				Movements actualMovement = Movements.NONE;
				float diff = 0;
				if (horizontalAxis < hAccValue && currentPosition >= 0) {
					//To the left
					actualMovement = Movements.LEFT;
					diff = -10f;
					DebugScript.self.addText("MOVE_LEFT");
				} else if (horizontalAxis > hAccValue && currentPosition <= 0) {
					//To the right
					actualMovement = Movements.RIGHT;
					diff = +10f;
					DebugScript.self.addText("MOVE_RIGHT");
				}
				//Using delay
				if(actualMovement != lastMovement || Time.time >= delayRepeatedMovements && actualMovement == lastMovement){
					currentPosition += diff;
					//					transform.Translate(currentPosition * Time.smoothDeltaTime, 0, 0);
//					transform.Translate(-Vector3.right * diff * Time.deltaTime);
//					transform.position = new Vector3 (currentPosition, transform.position.y, transform.position.z);
					transform.position = transform.position + Vector3.right * diff;
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
