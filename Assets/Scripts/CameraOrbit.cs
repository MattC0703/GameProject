using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform anchor;
    [SerializeField] private float orbitSpeed = 10;
    [SerializeField] private Vector3 orbitAxis = Vector3.up;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor == null) return;

        transform.RotateAround(anchor.position, orbitAxis, orbitSpeed * Time.deltaTime);
        transform.LookAt(anchor);
    }
}
