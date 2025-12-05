using UnityEngine;
using StarterAssets;

public class Snowball : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;

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
    }
}
