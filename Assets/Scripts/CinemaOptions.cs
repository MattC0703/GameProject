using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class CinemaOptions : MonoBehaviour
{
    public CinemachineCamera vCam;
    public CinemachineCamera freeCam;
    public float cameraSens = 5f;
    public float transTime = .3f;

    public bool isFreeCam = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vCam.Priority = 10;
        freeCam.Priority = 5;
    }

    // Update is called once per frame
    void Update()
    {
        CamSwitch();
    }

    void CamSwitch(){
        if(Input.GetMouseButtonDown(1)){
            if(!isFreeCam){
                freeCam.Priority = 11;
                isFreeCam = true;
            } 
            else if (isFreeCam){
                freeCam.Priority = 9;
                isFreeCam = false;
            }        
        
        }
    }
}
