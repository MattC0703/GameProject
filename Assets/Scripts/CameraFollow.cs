
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public UnityEngine.Vector3 offset = new UnityEngine.Vector3 (0f, 0f, 10f);
    public float smoothTime = .25f;
    public Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private void Update() {

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    }
}
