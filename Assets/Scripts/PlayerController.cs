using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float m_movSpeed;
	private Rigidbody rb;
    private Vector3 m_velocity;
    
	private float currentPosition = 0;
    
	private float hAccValue = 0.3f; //if windows = 0.
	private enum Movements { LEFT, RIGHT, NONE};
    private Movements lastMovement = Movements.NONE;
	private float delayRepeatedMovements;
	private float delayBetweenMovements;
	private float timeBetweenMovements = 0.5f;
	private float timeBetweenRepeatedMovements = 1f;
	private bool movingVertical = true;

    public void RotateRight(){
        rotate (true);
    }
    public void RotateLeft(){
        rotate (false);
    }
    private void rotate(bool right){
		movingVertical = !movingVertical;
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
		recenter();
    }
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody> ();
        m_velocity = new Vector3 (0f, 0.0f, m_movSpeed);
        rb.AddForce(m_velocity);
	}
	Transform lastPiece;

	private void recenter(){
		if(lastPiece == null){
			return;
		}


		Collider lastPieceCollider = lastPiece.GetComponent<Collider>();
//		lastPiece.gameObject.SetActive(false);
		float sizeX = lastPieceCollider.bounds.size.x;
		float sizeY = lastPieceCollider.bounds.size.y;
		float sizeZ = lastPieceCollider.bounds.size.z;

//		float diffX = sizeX / 4;

		Vector3 position = lastPiece.position - new Vector3(sizeX/2, 0, 0);
		float positionX = (position.x);

		transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
		
		//TODO: This is just for X
		if(movingVertical){
//			float positionX = position.x;
//			transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
		} else { //Moving Horizontal
//			position = lastPiece.position - new Vector3( sizeX/2, sizeY, sizeZ/2);
//			float positionZ = position.z;
//			transform.position = new Vector3(transform.position.x, transform.position.y, positionZ);
		}
		string positionStr = position.ToString();
		DebugScript.self.addText("Size: (" + sizeX + ", "+sizeY+", "+sizeZ+") " + positionStr);

	}
	void OnCollisionEnter(Collision collision){
		DebugScript.self.addText("COLLISION: " + collision.transform.tag);
		if(collision.transform.tag == "PIECE"){
			lastPiece = collision.transform;
			recenter();
		}
	}
//    void FixedUpdate(){
//        rb.velocity =  velocity;
//    }
    // Update is called once per frame
	void Update () {
		recenter ();
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
					currentPosition = 0; //TODO remove this line.
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
