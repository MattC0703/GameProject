
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CinemaOptions : MonoBehaviour
{
    public CinemachineCamera vCam;
    public CinemachineCamera freeCam;
    public CinemachineThirdPersonFollow vCamFollow;

    public Transform playerTransform;

    public CinemachineOrbitalFollow orbitFollow;
    public float cameraSens = 5f;
    public float transTime = .3f;

    public bool isFreeCam = false;

    public float zDeviation = 0f;
    public float initialZ;
    public Vector3 initialCamOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vCam.Priority = 10;
        freeCam.Priority = 5;
        initialZ = playerTransform.eulerAngles.z;
        
        initialCamOffset = vCamFollow.ShoulderOffset;
    }

    // Update is called once per frame
    void Update()
    {
        CamSwitch();
        CamAdjust();
    }

    void CamSwitch(){
        if(Input.GetMouseButtonDown(1)){
            if(!isFreeCam){
                orbitFollow.HorizontalAxis.Value = playerTransform.rotation.eulerAngles.y+90f;
                orbitFollow.VerticalAxis.Value = 28f;
                freeCam.Priority = 11;
                isFreeCam = true;
                // transform.rotation = new Quaternion(20, 0, 0, freeCam.transform.rotation.w);
            } 
            else if (isFreeCam){
                freeCam.Priority = 9;
                isFreeCam = false;
            }        
            
        }
    }

    //adjusts the camera when the player is rotating to retain visual clarity at different angles
    void CamAdjust(){
        float currentZ = playerTransform.eulerAngles.z;

        float deviation = Mathf.DeltaAngle(initialZ, currentZ);
        
        if(Mathf.Abs(deviation)< .05) deviation = 0;

        zDeviation = Mathf.Max(0f, deviation);

        vCamFollow.ShoulderOffset = new Vector3(initialCamOffset.x-zDeviation, initialCamOffset.y-zDeviation/6, vCamFollow.ShoulderOffset.z);
    }
}
