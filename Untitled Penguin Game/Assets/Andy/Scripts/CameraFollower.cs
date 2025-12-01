using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 cameraOffset;
    public float followSpeed = 10f;

    [Header("Camera Bounds")]
    public float xMin = 0f;
    public float xMax = 100f;
    public float yMin = -10f;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + cameraOffset;

        float clampedX = Mathf.Clamp(targetPos.x, xMin, xMax);
        float clampedY = Mathf.Max(targetPos.y, yMin);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, targetPos.z);

        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}