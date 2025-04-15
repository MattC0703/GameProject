using UnityEngine;

public class AirMovement : MonoBehaviour
{
    public FixCarOrientation fixCarRef;
    public Rigidbody pb;
    [SerializeField] private float torqueForce = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        airTilt();
    }

    void airTilt(){
        if(fixCarRef.isGrounded != true){
            if(Input.GetAxis("Tilting") > 0){
                // Debug.Log("pressed control");
                Vector3 currentTransform = pb.transform.eulerAngles;
                currentTransform.z += torqueForce*Time.deltaTime;
                pb.transform.eulerAngles = currentTransform;
            } else if(Input.GetAxis("Tilting")<0){
                // Debug.Log("pressed shift");
                Vector3 currentTransform = pb.transform.eulerAngles;
                currentTransform.z -= torqueForce*Time.deltaTime;
                pb.transform.eulerAngles = currentTransform;
            }
        }
    }
}
