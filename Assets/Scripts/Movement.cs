using System.Numerics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody playerBody;
    [SerializeField] public float forceAmount = 0;
    [SerializeField] public float rotateAmount = 0;

    public float rotMaxAccel = 200f;
    public float rotAccelSpeed = 100f;
    public float rotDeccelSpeed = 100f;
    [SerializeField] private float rotCurrentAccel = 1f;

    public float mod = 0;

    enum RotationDirection { None, Left, Right }
    RotationDirection lastDirection = RotationDirection.None;


    [SerializeField] private bool isRotating = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerBody.AddForce(UnityEngine.Vector3.Normalize(UnityEngine.Vector3.right) * forceAmount, ForceMode.VelocityChange);
        if(Input.GetButtonDown("Left") || Input.GetButtonDown("Right")){
            isRotating = true;
        }
        if(Input.GetButton("Left")){
            LeftRotationAccel();
            // Debug.Log("the button works");
            isRotating = true;
            lastDirection = RotationDirection.Left;
            if(rotCurrentAccel >= 40)
                rotCurrentAccel = 40;
            // transform.Rotate(new Vector3 (transform.rotation.x, transform.rotation.y-rotCurrentAccel, transform.rotation.z));
        }
        if(Input.GetButton("Right")){
            RightRotationAccel();
            // Debug.Log("the button works");
            isRotating = true;
            lastDirection = RotationDirection.Right;
            // transform.Rotate(new Vector3 (transform.rotation.x, transform.rotation.y-rotCurrentAccel, transform.rotation.z));
        }

        
        if(!Input.GetButton("Left") && !Input.GetButton("Right")){
        isRotating = false;
        }

        if(!isRotating){
            RotationDeccel();
        }

        // transform.Rotate(new Vector3 (transform.rotation.x, transform.rotation.y+rotCurrentAccel, transform.rotation.z));
        UnityEngine.Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += rotCurrentAccel*Time.deltaTime;
        transform.eulerAngles = currentRotation;

    }

    void RightRotationAccel(){
        rotCurrentAccel += rotAccelSpeed * Time.deltaTime;
        rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, -40, rotMaxAccel);
    }
    void LeftRotationAccel(){
        rotCurrentAccel -= rotAccelSpeed * Time.deltaTime;
        rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, -400, rotMaxAccel);
    }
    void RotationDeccel(){
        switch (lastDirection)
    {
        case RotationDirection.Right:
            rotCurrentAccel -= rotDeccelSpeed * Time.deltaTime;
            rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, 0, rotMaxAccel);
            break;
        case RotationDirection.Left:
            rotCurrentAccel += rotDeccelSpeed * Time.deltaTime; 
            rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, -rotMaxAccel, 0);
            break;
    }
    }
}
