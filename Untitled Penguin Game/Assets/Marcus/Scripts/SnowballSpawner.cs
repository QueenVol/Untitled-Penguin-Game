using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    [SerializeField] private GameObject snowballPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnForce = 0f; // Initial push if needed
    [SerializeField] private float spawnWidth = 10f; // Horizontal range for spawning

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnSnowball();
            _timer = 0f;
        }
    }

    private void SpawnSnowball()
    {
        if (snowballPrefab != null)
        {
            // Calculate random position offset
            float randomX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
            Vector3 spawnPos = transform.position + transform.right * randomX;

            GameObject snowball = Instantiate(snowballPrefab, spawnPos, transform.rotation);
            if (spawnForce > 0)
            {
                Rigidbody rb = snowball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * spawnForce, ForceMode.Impulse);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position - transform.right * spawnWidth / 2f, transform.position + transform.right * spawnWidth / 2f);
    }
}
