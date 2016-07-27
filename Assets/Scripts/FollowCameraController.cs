using UnityEngine;
using System.Collections;

public class FollowCameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
    float y = 19;
    float x = 0;
    float z = 36;
    public float smoothFactor;
    private float realSmoothFactor;
    //Time we should wait for turning and reset
    float smoothFactorDelay = 1f;
    float nextSmooth = 0;

    // Use this for initialization
    void Start () {
        //offset = transform.position - player.transform.position;
        buildOffset(new Vector3(0, 0, 1));
    }

    //Smooth factor reset.
    void Update(){
        //This stabilize the smooth factor to 50 (so the camera does not jump) 
        if(Time.time > nextSmooth){
            realSmoothFactor = 50f;
        }
    }
    // Update is called once per frame
    void LateUpdate () {

        Vector3 newPosition = player.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * realSmoothFactor);

        //transform.position = newPosition;

        transform.LookAt (player.transform);
    }

    public void buildOffset(Vector3 direction){
        realSmoothFactor = smoothFactor;
        nextSmooth = Time.time + smoothFactorDelay;

        if(direction.x != 0){
            this.offset = new Vector3 ( (direction.x > 0 ? -z : z ), y, x);
        } else if(direction.z != 0){
            this.offset = new Vector3 ( x, y, (direction.z > 0 ? -z : z ));
        }
    }
}
