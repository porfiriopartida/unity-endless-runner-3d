using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float m_movSpeed;
	private Rigidbody rb;
	private Vector3 m_velocity;
	public Transform lastPiece;
	public GameObject [] piecesPool;
	public GameObject nextPiece;
	GameObject piecesBucket;

    
	private float hAccValue = 0.3f; //if windows = 0.
	private enum Movements { LEFT, RIGHT, NONE};
    private Movements lastMovement = Movements.NONE;
	private float delayRepeatedMovements;
	private float delayBetweenMovements;
	private float timeBetweenMovements = 0.5f;
	private float timeBetweenRepeatedMovements = 1f;
	public bool movingVertical;
	public float centerFactor;
	public float step  = 10f;
	public int pieces = 0;
	private const int FRONT = 0, RIGHT = 1, BACK = 2, LEFT = 3;
	public int facing = 0; //starts facing forward +Z
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
			facing++;
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
			facing--;
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
		if(facing < 0){
			facing += 4; // -1 + 4 = 3 = LEFT
		}
		facing = Mathf.Abs( facing % 4); // 1 = 5 = 7 = Going right

        float angle = (right ? 90 : -90);
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
	void Awake(){
		piecesBucket = GameObject.Find("Pieces");
	}

	public void recenter(){
		if(lastPiece == null){
			return;
		}

		Collider lastPieceCollider = lastPiece.GetComponent<Collider>();
		Transform center = lastPiece.transform.Find ("center");
		Vector3 position = center.position;
		if(movingVertical){
			float positionX = position.x + (centerFactor * ( facing == PlayerController.FRONT ? 1:-1 )) ;
			transform.position = new Vector3(positionX , transform.position.y, transform.position.z);
		} else { //Moving Horizontal
			float positionZ = position.z + (centerFactor * ( facing == PlayerController.LEFT ? 1:-1 )) ;
			transform.position = new Vector3(transform.position.x, transform.position.y, positionZ);
		}
	}
	void OnTriggerEnter(Collider collider){
		DebugScript.self.addText("COLLISION: " + collider.transform.tag);
		if(collider.transform.tag == "PIECE"){
			collider.enabled = false; //Prevent recollide.

			pieces++;
			if(pieces < 2){
				recenter (); //Recenter in the first w/o turn.
			}

			//Destroy the last touched piece.
			if(lastPiece != null){
				Destroy (lastPiece.gameObject);
			}
			//New last touched piece.
			lastPiece = collider.transform;

			//The farest piece is where we need to put a new object
			//New object (randomly) created from the pool.
			GameObject newPiece = GameObject.Instantiate(piecesPool[0]); //TODO: Ramdomize
			if(piecesBucket != null){
				newPiece.transform.parent = piecesBucket.transform;
			}
			//Repositioning the new piece
			float sizeZ = nextPiece.GetComponent<Collider>().bounds.size.z;
			newPiece.transform.position = nextPiece.transform.position + new Vector3(0, 0, sizeZ); //TODO: More than just forward
			//Newest piece is now the farest.
			nextPiece = newPiece;
		}
	}
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
				if (horizontalAxis < hAccValue && centerFactor > -step) {
					//To the left
					actualMovement = Movements.LEFT;
					diff = -step;
					DebugScript.self.addText("MOVE_LEFT");
				} else if (horizontalAxis > hAccValue && centerFactor < step) {
					//To the right
					actualMovement = Movements.RIGHT;
					diff = step;
					DebugScript.self.addText("MOVE_RIGHT");
				}
				//Using delay
				if(actualMovement != lastMovement || Time.time >= delayRepeatedMovements && actualMovement == lastMovement){
					centerFactor += diff;
					delayRepeatedMovements = Time.time + timeBetweenRepeatedMovements;
					delayBetweenMovements = Time.time + timeBetweenMovements;
					lastMovement = actualMovement;
					recenter ();
				}
			} else {
				delayRepeatedMovements = 0;
			}
        }
    }
}
