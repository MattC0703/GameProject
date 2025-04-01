using UnityEngine;

public class TurnWheels : MonoBehaviour
{
    public Transform leftWheel;
    public Transform rightWheel;

    [SerializeField] private float maxRot = 30f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float resetSpeed = 5f;
    [SerializeField] private float currentAngle = 90f;
    public bool isRotating = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Left") || Input.GetButton("Right")){
            isRotating = true;
        } else {
            isRotating = false;
        }

        if(isRotating && Input.GetButton("Left") && !Input.GetButton("Right")){
            currentAngle = Mathf.Lerp(currentAngle, 60, Time.deltaTime * turnSpeed);

        } else if (isRotating && Input.GetButton("Right") && !Input.GetButton("Left")){
            currentAngle = Mathf.Lerp(currentAngle, 120, Time.deltaTime * turnSpeed);

        } else {
            currentAngle = Mathf.Lerp(currentAngle, 90, Time.deltaTime * turnSpeed);

        }   
        ApplyRotation();
    }
    void ApplyRotation(){
            leftWheel.localRotation = Quaternion.Euler(0, currentAngle, 0);
            rightWheel.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }
}
