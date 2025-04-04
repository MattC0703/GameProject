using System;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Cinemachine;
using Unity.VisualScripting;
// using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody playerBody;
    [SerializeField] public float forceAmount = 0;
    [SerializeField] public float rotateAmount = 0;

    public CinemachineCamera vCam;
    public CinemachineThirdPersonFollow thirdPersonFollow;
    public float camDefaultDistance = 4f;
    public float camAccel = 1.5f;
    public float camDecel = 1.5f;

    public float rotMaxAccel = 200f;
    public float rotAccelSpeed = 100f;
    public float rotDeccelSpeed = 100f;
    [SerializeField] private float rotCurrentAccel = 1f;

    public float mod = 0;

    public float maxAccel = 700f;
    public float accelSpeed = 200f;
    public float originalAccelSpeed = 20f;
    public float deccelSpeed = 100f;
    public float currentAccel = 0f;

    public float initialPullStrengthx = Mathf.Clamp(.1f, .1f, 100f);
    public float initialPullStrengthz = Mathf.Clamp(.1f, .1f, 100f);

    public float turningDampValue = 5f;

    public bool isDrifting = false;

    public float camLerpSpeed = 5f;

    public ParticleSystem staticDriftDustLeft;
    public ParticleSystem staticDriftDustRight;
    public ParticleSystem dynamicDriftDustLeft;
    public ParticleSystem dynamicDriftDustRight;
    [SerializeField] private bool hasParticles = false;

    enum RotationDirection { None, Left, Right }
    RotationDirection lastDirection = RotationDirection.None;

    enum goStop {None, Forward, Back}
    goStop momentumDirection = goStop.None;


    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool isAccelerating = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float originalRotAccel;
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        originalRotAccel = rotMaxAccel;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // fixCarOrientation();

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
            } else if (currentAccel > 0 && currentAccel < maxAccel/2){
                accelSpeed = originalAccelSpeed;
            } else if (currentAccel >= maxAccel/2  && currentAccel < maxAccel/1.35){
                accelSpeed = originalAccelSpeed/3;
            } else if (currentAccel >= maxAccel/1.35 ){
                accelSpeed = originalAccelSpeed/20;
            }
            Accelerate();
        }
        if(Input.GetButton("Down")){
            momentumDirection = goStop.Back;
            isAccelerating = true;
            if(currentAccel > 0){
                accelSpeed = deccelSpeed+200;
                isAccelerating = false;
            } else if (currentAccel < 0 && currentAccel > maxAccel/2*-1){
                accelSpeed = originalAccelSpeed;
                isAccelerating = true;
            } else if (currentAccel < maxAccel/2*-1 && currentAccel > maxAccel/1.35*-1){
                accelSpeed = originalAccelSpeed/3;
            } else if (currentAccel <= maxAccel/1.35*-1){
                accelSpeed = originalAccelSpeed/20;
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
            
            playerBody.angularVelocity = Vector3.Lerp(playerBody.angularVelocity, Vector3.zero, turningDampValue * Time.deltaTime); 
            //if player isn't turning, naturally straighten car
        }
        if(!isAccelerating && currentAccel > 0){
            Decelerate();
            if(playerBody.linearVelocity.magnitude > currentAccel)
            playerBody.linearVelocity = playerBody.linearVelocity.normalized * currentAccel; 
        }
        if(!isAccelerating && currentAccel < 0){
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
        // Debug.Log(playerBody.linearVelocity);
        SmoothTurn();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            isDrifting = true;
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isDrifting = false;
        }
        if(Input.GetKey(KeyCode.Space)){
            isDrifting = true;
        }
        Drift();

        if(staticDriftDustLeft != null && staticDriftDustRight != null && dynamicDriftDustLeft != null && dynamicDriftDustRight != null && hasParticles == true){
            if(currentAccel >= 23 && rotCurrentAccel >= 90 || rotCurrentAccel > 100 || currentAccel >= 23 && rotCurrentAccel <= -90 || rotCurrentAccel < -100){
                if(!staticDriftDustLeft.isPlaying){
                    staticDriftDustLeft.Play(); staticDriftDustRight.Play(); dynamicDriftDustLeft.Play(); dynamicDriftDustRight.Play();
                }
            }
            else {
                if(staticDriftDustLeft.isPlaying){
                    staticDriftDustLeft.Stop(); staticDriftDustRight.Stop(); dynamicDriftDustLeft.Stop(); dynamicDriftDustRight.Stop();
                }
            }
        }
    }

    private float camFarAllow = 6f;
    private float camShortAllow = 1f;
    private float originalCamFarAllow = 6f;
    private float originalCamShortAllow = 1f;
    void RightRotationAccel(){
        rotCurrentAccel += rotAccelSpeed * Time.deltaTime;
        rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, -40, rotMaxAccel);

        thirdPersonFollow.CameraDistance += camAccel * Time.deltaTime;
        thirdPersonFollow.CameraDistance = Mathf.Clamp(thirdPersonFollow.CameraDistance, camShortAllow, camFarAllow);
    }
    void LeftRotationAccel(){
        rotCurrentAccel -= rotAccelSpeed * Time.deltaTime;
        rotCurrentAccel = Mathf.Clamp(rotCurrentAccel, -rotMaxAccel, rotMaxAccel);

        thirdPersonFollow.CameraDistance -= camAccel * Time.deltaTime;
        thirdPersonFollow.CameraDistance = Mathf.Clamp(thirdPersonFollow.CameraDistance, camShortAllow, camFarAllow);
    }
    void RotationDeccel(){
            if(thirdPersonFollow.CameraDistance >= 4f){
            thirdPersonFollow.CameraDistance -= camDecel * Time.deltaTime;
            thirdPersonFollow.CameraDistance = Mathf.Clamp(thirdPersonFollow.CameraDistance, 4f, camFarAllow); }
            
            if(thirdPersonFollow.CameraDistance <= 4f){
            thirdPersonFollow.CameraDistance += camDecel * Time.deltaTime;
            thirdPersonFollow.CameraDistance = Mathf.Clamp(thirdPersonFollow.CameraDistance, camShortAllow, 4f); }

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

    private Coroutine camLerpCoroutine;  // Store the reference to the coroutine
    void Drift(){
        if(isDrifting){
            camFarAllow = 8f;
            camShortAllow = -1f;

            rotMaxAccel *= 2;

            if(camLerpCoroutine != null){
                StopCoroutine(camLerpCoroutine);
                camLerpCoroutine = null;
            }
        } 
        if(!isDrifting){
            // camFarAllow = 6f;
            // camShortAllow = 1f;
            // camFarAllow = Mathf.Lerp(camFarAllow, originalCamFarAllow, camLerpSpeed * Time.deltaTime);
            // camShortAllow = Mathf.Lerp(camShortAllow, originalCamFarAllow, camLerpSpeed * Time.deltaTime);

            if(camLerpCoroutine == null){
                camLerpCoroutine = StartCoroutine(SmoothCameraTransition());
            }

            rotMaxAccel = originalRotAccel;
        }
    }

    IEnumerator SmoothCameraTransition(){
    float lerpProgress = 0f;


    float currentCamFar = camFarAllow;
    float currentCamShort = camShortAllow;

    // Lerp over time
    while (lerpProgress < 1f) {
        lerpProgress += Time.deltaTime * camLerpSpeed;

        // Lerp between the current and original values
        camFarAllow = Mathf.Lerp(currentCamFar, originalCamFarAllow, lerpProgress);
        camShortAllow = Mathf.Lerp(currentCamShort, originalCamShortAllow, lerpProgress);

        yield return null;  // Wait for the next frame
    }

    // Once Lerp is done, hard set the final values
    camFarAllow = originalCamFarAllow;
    camShortAllow = originalCamShortAllow;

    // Reset coroutine reference
    camLerpCoroutine = null;
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


    // void fixCarOrientation(){
    //     Vector3 carRotation = transform.eulerAngles;
    //     Debug.Log(carRotation);
    //     Debug.Log(initialPullStrengthx);

        

    //     if(carRotation.x > 0 && carRotation.x < 180){
    //         carRotation.x = Mathf.Clamp(transform.eulerAngles.x, 0, 359.5f);
    //         carRotation.x -= initialPullStrengthx;
    //         transform.eulerAngles = carRotation;

    //         if(initialPullStrengthx < 50)
    //         initialPullStrengthx *= 1.1f;
    //     } 
    //     else if (carRotation.x > 180 && carRotation.x < 359.5f){
    //         carRotation.x = Mathf.Clamp(carRotation.x, 0, 359.5f);
    //         carRotation.x += initialPullStrengthx;
    //         transform.eulerAngles = carRotation;

    //         if(initialPullStrengthx < 50)
    //         initialPullStrengthx *= 1.1f;
    //     } 
    //     if (carRotation.x >= 359.5f){
    //         carRotation.x = Mathf.Clamp(carRotation.x, 0, .5f);
    //         carRotation.x = 0;
    //         transform.eulerAngles = carRotation;
    //         initialPullStrengthx = .1f;
    //     }
    //     if (carRotation.x <= .5f){
    //         carRotation.x = Mathf.Clamp(carRotation.x, 0, .5f);
    //         carRotation.x = 0;
    //         transform.eulerAngles = carRotation;
    //         initialPullStrengthx = .1f;
    //     }

    //     if(carRotation.z > 0 && carRotation.z < 180){
    //         carRotation.z = Mathf.Clamp(transform.eulerAngles.z, 0, 359.5f);
    //         carRotation.z -= initialPullStrengthz;
    //         transform.eulerAngles = carRotation;

    //         if(initialPullStrengthz < 50)
    //         initialPullStrengthz *= 1.1f;
    //     } 
    //     else if (carRotation.z > 180 && carRotation.z < 359.5f){
    //         carRotation.z = Mathf.Clamp(carRotation.z, 0, 359.5f);
    //         carRotation.z += initialPullStrengthz;
    //         transform.eulerAngles = carRotation;

    //         if(initialPullStrengthz < 50)
    //         initialPullStrengthz *= 1.1f;
    //     } 
    //     if (carRotation.z >= 395.5f){
    //         carRotation.z = Mathf.Clamp(carRotation.z, 0, .5f);
    //         carRotation.z = 0;
    //         transform.eulerAngles = carRotation;
    //         initialPullStrengthz = .1f;
    //     } 
    //     if (carRotation.z <= .5f){
    //         carRotation.z = Mathf.Clamp(carRotation.x, 0, .5f);
    //         carRotation.z = 0;
    //         transform.eulerAngles = carRotation;
    //         initialPullStrengthz = .1f;
    //     }
    // }

    void SmoothTurn(){
        Vector3.Lerp(playerBody.linearVelocity, transform.forward * currentAccel, 1f);
    }


}
