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
    int currentPosition = 0;
    // Update is called once per frame
    void Update () {
        float horizontalAxis = Input.GetAxis ("Horizontal");
        bool buttonHorizontal = Input.GetButtonDown ("Horizontal");
        //float moveVertical = Input.GetAxis ("Vertical");

        if (buttonHorizontal) {
            if (horizontalAxis < 0 && currentPosition >= 0) {
                currentPosition -= 10;
            } else if (horizontalAxis > 0 && currentPosition <= 0) {
                currentPosition += 10;
            }
            transform.position = new Vector3 (currentPosition, transform.position.y, transform.position.z);
        }
    }
}
