using System;
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

    public float maxAccel = 700f;
    public float accelSpeed = 200f;
    public float deccelSpeed = 100f;
    public float currentAccel = 0f;

    enum RotationDirection { None, Left, Right }
    RotationDirection lastDirection = RotationDirection.None;

    enum goStop {None, Forward, Back}
    goStop momentumDirection = goStop.None;


    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool isAccelerating = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // playerBody.AddForce(UnityEngine.Vector3.Normalize(UnityEngine.Vector3.right) * forceAmount, ForceMode.VelocityChange);
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
        if(Input.GetButtonDown("Up") || Input.GetButtonDown("Down")){
            isAccelerating = true;
        }
        if(Input.GetButton("Up")){
            momentumDirection = goStop.Forward;
            isAccelerating = true;
            if(currentAccel < 0){
                accelSpeed = deccelSpeed+200;
            } else if (currentAccel > 0){
                accelSpeed = 50;
            }
            Accelerate();
        }
        if(Input.GetButton("Down")){
            momentumDirection = goStop.Back;
            isAccelerating = true;
            if(currentAccel > 0){
                accelSpeed = deccelSpeed+200;
                isAccelerating = false;
            } else if (currentAccel < 0){
                accelSpeed = 50;
                isAccelerating = true;
            }
            Accelerate();
        }

        
        if(!Input.GetButton("Left") && !Input.GetButton("Right")){
            isRotating = false;
        }
        if(!Input.GetButton("Up") && !Input.GetButton("Down")){
            isAccelerating = false;
        }

        if(!isRotating){
            RotationDeccel();
        }
        if(!isAccelerating && currentAccel > 0){
            Decelerate();
            if(playerBody.linearVelocity.magnitude > currentAccel)
            playerBody.linearVelocity = playerBody.linearVelocity.normalized * currentAccel; 
        }
        if(!isAccelerating&& currentAccel < 0){
            Decelerate();
            if(playerBody.linearVelocity.magnitude*-1 < currentAccel)
            playerBody.linearVelocity = playerBody.linearVelocity.normalized * currentAccel*-1; 
        }

        // transform.Rotate(new Vector3 (transform.rotation.x, transform.rotation.y+rotCurrentAccel, transform.rotation.z));
        UnityEngine.Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += rotCurrentAccel*Time.deltaTime;
        transform.eulerAngles = currentRotation;

        ClampMaxVelocity();
        // Vector3 currentLocation = transform.right;
        playerBody.AddForce(transform.right * currentAccel * Time.deltaTime, ForceMode.VelocityChange);
        Debug.Log(playerBody.linearVelocity);

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
    void Accelerate(){
        switch(momentumDirection)
    {
        case goStop.Forward:
            currentAccel += accelSpeed * Time.deltaTime;
            currentAccel = Mathf.Clamp(currentAccel, -200, maxAccel);
            break;
        case goStop.Back:
            currentAccel -= accelSpeed * Time.deltaTime;
            currentAccel = Mathf.Clamp(currentAccel, -50, 400);
            break;        
    }
    }
    void Decelerate(){
        switch(momentumDirection)
    {
        case goStop.Forward:
            currentAccel -= deccelSpeed * Time.deltaTime;
            currentAccel = Mathf.Clamp(currentAccel, 0, maxAccel);
            break;
        case goStop.Back:
            currentAccel += deccelSpeed * Time.deltaTime;
            currentAccel = Mathf.Clamp(currentAccel, -50, 0);
            break;        
    }
    }
    
    void ClampMaxVelocity(){
        Vector3 velocity = playerBody.linearVelocity;
        if(velocity.magnitude > maxAccel){
            playerBody.linearVelocity = velocity.normalized * maxAccel;
        }
    }
}
