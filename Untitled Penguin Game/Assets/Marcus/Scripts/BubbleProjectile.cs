using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    private Rigidbody _rigidbody;
    private bool _isStuck = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isStuck) return;

        // Ignore collision with the player initially to avoid getting stuck immediately if spawned inside
        if (collision.gameObject.CompareTag("Player")) return;

        // Stick to the surface
        _isStuck = true;
        _rigidbody.isKinematic = true;
        
        // Stop velocity completely
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        // Optional: Parent to the object we hit so we move with it
        transform.SetParent(collision.transform);
    }
}
