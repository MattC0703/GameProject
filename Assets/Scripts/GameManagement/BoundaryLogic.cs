using UnityEngine;

public class BoundaryLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("something touched me");
        if(other.CompareTag("Player"))
        {
            Debug.Log("player touched me");
            PlayerCheckpoints checkpointHandler = other.GetComponent<PlayerCheckpoints>();
            if(checkpointHandler != null && checkpointHandler.getLastCheckpoint() != null){
                checkpointHandler.TeleportToCheckpoint();
            }
        }
    }
}
