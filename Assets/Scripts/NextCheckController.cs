using UnityEngine;
using System.Collections;

public class NextCheckController : MonoBehaviour {
    public static string LAST_PIECE = "FRONT";
    public static GameObject toBeDestroyed = null;
	public GameObject m_frontPrefab, m_leftPrefab;
	public static GameObject LAST_PIECE_OBJECT;
    public static int piecesCounter = 0;

    GameObject piecesWrapper;

    void Start(){
        if (LAST_PIECE_OBJECT == null) {
            LAST_PIECE_OBJECT = GameObject.Find ("Last");
        }
        piecesWrapper = GameObject.Find ("Pieces");
    }

    void OnTriggerEnter(Collider other) {
        GameObject gameObject = other.gameObject;
        if(gameObject != null && gameObject.CompareTag("Player")){
            addNewPiece ();

            destroyFirstPiece ();
        }
	}
	private float getDepth(){
		Collider collider = LAST_PIECE_OBJECT.GetComponent<Collider>();
		if(collider == null){
			collider = LAST_PIECE_OBJECT.GetComponentInChildren<Collider>();
		}
		switch(LAST_PIECE){
			case "RIGHT":
				return collider.bounds.size.x;
			case "FRONT":
				return collider.bounds.size.z - 1;
			default:
				throw new UnityException("Invalid piece called.");
		}
	}

	GameObject getNewPiece(string sValue){
		switch(sValue){
			case "RIGHT":
				return Instantiate (m_leftPrefab);
			case "FRONT":
				return Instantiate (m_frontPrefab);
			default:
				throw new UnityException("Invalid piece called.");
		}
	}
    void addNewPiece (){
		string sValue = getRandomPiece ();
		print (sValue);
		
		//TODO: Change depending on last piece value.
		GameObject newFloor = getNewPiece(sValue);

		repositionNewFloor(newFloor);
        //LAST_PIECE_OBJECT.name = "NotLast";
		//New last piece added.
		LAST_PIECE = sValue;
		LAST_PIECE_OBJECT = newFloor;
    }
	void repositionNewFloor(GameObject newFloor){
		float offset = getDepth();
		//Reubicate the new piece in front of the last piece.
		Vector3 newPosition;
		switch(LAST_PIECE){
			case "RIGHT":
				newPosition = new Vector3(LAST_PIECE_OBJECT.transform.position.x + offset, LAST_PIECE_OBJECT.transform.position.y, LAST_PIECE_OBJECT.transform.position.z);
				break;
			case "FRONT":
				newPosition = new Vector3(LAST_PIECE_OBJECT.transform.position.x, LAST_PIECE_OBJECT.transform.position.y, LAST_PIECE_OBJECT.transform.position.z + offset);
				break;
			default:
				throw new UnityException("Invalid piece called.");
		}
		newFloor.transform.position = newPosition;
		newFloor.name = "Piece" +(++piecesCounter);
		newFloor.transform.parent = piecesWrapper.transform;
	}
	void destroyFirstPiece(){
        if (toBeDestroyed != null) {
            Destroy (toBeDestroyed);
            toBeDestroyed = null;
        }
        toBeDestroyed = gameObject.transform.parent.gameObject;
        Destroy (gameObject); //Destroying the collider.
    }
    string getRandomPiece(){
        int value = Mathf.RoundToInt(Random.value * 2);
        string sValue;
        switch(value){
//            case 1:
//                sValue = "LEFT";
//                break;
            case 1:
                sValue = "RIGHT";
                break;
            default:
                sValue = "FRONT";
                break;
		}
		sValue = "RIGHT";
		return sValue;
	}
}
