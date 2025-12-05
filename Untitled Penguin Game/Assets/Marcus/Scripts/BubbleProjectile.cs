using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject popVfxPrefab;
    [SerializeField] private float wobbleSpeed = 10f;
    [SerializeField] private float wobbleAmount = 0.05f;

    private Rigidbody _rigidbody;
    private bool _isStuck = false;
    private Vector3 _originalScale;
    private float _timeOffset;

    public bool IsStuck => _isStuck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
        _timeOffset = Random.Range(0f, 100f);
    }

    private void Start()
    {
        _rigidbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        // Simple wobble effect
        float wobble = Mathf.Sin((Time.time + _timeOffset) * wobbleSpeed) * wobbleAmount;
        transform.localScale = _originalScale + Vector3.one * wobble;
    }

    public void Pop()
    {
        if (popVfxPrefab != null)
        {
            Instantiate(popVfxPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the player - if so, destroy the bubble and do nothing else
        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player") || collision.gameObject.GetComponentInParent<StarterAssets.ThirdPersonController>() != null)
        {
            Pop();
            return;
        }

        if (_isStuck) return;

        // Stick to the surface
        _isStuck = true;
        _rigidbody.isKinematic = true;
        
        // Stop velocity completely
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        // Optional: Parent to the object we hit so we move with it
        // Note: Parenting can cause scale distortion if the parent has non-uniform scale
        // transform.SetParent(collision.transform);
        transform.parent = null; // Ensure we don't have a parent to avoid scale issues
    }
}
