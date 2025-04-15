using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{

    public GameObject pauseMenu;
    public UIDocument uiDoc;
    public static bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pauseUnpause();
    }

    void pauseUnpause(){
        if(Input.GetButtonDown("Pause") && !isPaused){
            
            isPaused = true;
            Debug.Log(isPaused);
        
        } else if(isPaused && Input.GetButtonDown("Pause")){

            isPaused = false;
            Debug.Log(isPaused);
        
        }

        if(isPaused){
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void Unstuck(){
        
    }
}
