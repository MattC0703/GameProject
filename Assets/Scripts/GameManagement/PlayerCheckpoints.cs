using UnityEngine;

public class PlayerCheckpoints : MonoBehaviour
{
    private Transform lastCheckpoint;

    private Rigidbody rb;
    public FadeToBlack fader;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    public void TeleportToCheckpoint()
    {
        if (lastCheckpoint == null) return;


        StartCoroutine(fader.FadeOutIn(() =>
    {
        transform.position = lastCheckpoint.position;
        transform.rotation = lastCheckpoint.rotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }));
    }

    public Transform getLastCheckpoint(){
        return lastCheckpoint;
    }
}
