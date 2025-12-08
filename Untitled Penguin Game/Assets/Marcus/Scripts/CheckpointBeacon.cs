using UnityEngine;

public class CheckpointBeacon : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How fast the object rotates around the Y axis.")]
    public float rotationSpeed = 30f;
    
    [Tooltip("How fast the object moves up and down.")]
    public float bobSpeed = 1f;
    
    [Tooltip("How far the object moves up and down.")]
    public float bobHeight = 0.5f;

    private Vector3 _startLocalPosition;

    private void Start()
    {
        _startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Rotate around local Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        // Bob up and down relative to parent
        if (bobHeight > 0)
        {
            float newY = _startLocalPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.localPosition = new Vector3(_startLocalPosition.x, newY, _startLocalPosition.z);
        }
    }
}

