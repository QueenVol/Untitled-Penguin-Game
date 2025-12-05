using UnityEngine;
using StarterAssets;

public class Snowball : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float bounceForce = 10f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<ThirdPersonController>();
            if (player == null)
            {
                player = collision.gameObject.GetComponentInParent<ThirdPersonController>();
            }

            if (player != null)
            {
                player.Respawn();
                // Destroy snowball on impact
                Destroy(gameObject);
            }
        }
        // Check if we hit a bubble
        else if (collision.gameObject.TryGetComponent<BubbleProjectile>(out BubbleProjectile bubble))
        {
            if (bubble.IsStuck && _rigidbody != null)
            {
                // Simple vertical bounce + slight random horizontal variation
                Vector3 bounceDir = Vector3.up + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // Reset vertical velocity
                _rigidbody.AddForce(bounceDir.normalized * bounceForce, ForceMode.Impulse);
                
                bubble.Pop();
            }
        }
    }
}
