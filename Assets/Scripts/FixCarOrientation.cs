using System;
using Unity.Mathematics;
using UnityEngine;

public class FixCarOrientation : MonoBehaviour
{
   public bool isGrounded = true;

   [SerializeField] private float maxGravity = 50f;
   [SerializeField] private float minGravity = 10f;
   [SerializeField] private float gravityGrowthRate = 10f;
   [SerializeField] private float currentGravity;
   
   public Rigidbody playerBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGravity = minGravity;
    }

    // Update is called once per frame
    void Update()
    {
        FixOrientation();
        CheckGround();
        // Gravity();
    }
    void FixedUpdate()
    {
        Gravity();
    }

    void FixOrientation(){
        // RaycastHit hit;
        
        // if(Physics.Raycast(transform.position, Vector3.down, out hit, 5f)){
        //     Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2f);
        // }
    }



    void CheckGround(){
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
            isGrounded = true;
            currentGravity = minGravity;
        } else {
            isGrounded = false;
        }
    }

    void Gravity(){
        currentGravity += gravityGrowthRate * Time.deltaTime;
        currentGravity = Mathf.Clamp(currentGravity, minGravity, maxGravity);

        playerBody.AddForce(Vector3.down * currentGravity, ForceMode.Acceleration);
    }
}
