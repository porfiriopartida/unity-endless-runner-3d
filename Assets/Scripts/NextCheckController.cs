using UnityEngine;
using System.Collections;

public class NextCheckController : MonoBehaviour {
    public static string LAST_PIECE = "FRONT";
    public static GameObject toBeDestroyed = null;
    public GameObject validPrefab;
    public static GameObject LAST_PIECE_OBJECT;
    public static int piecesCounter = 0;
    private GameObject piecesWrapper;

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

    void addNewPiece (){
        string sValue = getRandomPiece ();
        LAST_PIECE = sValue;

        //TODO: Change depending on last piece value.
        GameObject newFloor = Instantiate (validPrefab);

        float offset = 90f;
        //Reubicate the new piece in front of the last piece.
        newFloor.transform.position = new Vector3(LAST_PIECE_OBJECT.transform.position.x, LAST_PIECE_OBJECT.transform.position.y, LAST_PIECE_OBJECT.transform.position.z + offset);
        newFloor.name = "Piece" +(++piecesCounter);
        newFloor.transform.parent = piecesWrapper.transform;
        //LAST_PIECE_OBJECT.name = "NotLast";
        //New last piece added.
        LAST_PIECE_OBJECT = newFloor;
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
            case 1:
                sValue = "LEFT";
                break;
            case 2:
                sValue = "RIGHT";
                break;
            case 0:
            default:
                sValue = "FRONT";
                break;
        }
        return sValue;
    }
}
