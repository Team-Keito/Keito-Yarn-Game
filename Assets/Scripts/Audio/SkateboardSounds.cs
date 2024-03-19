using UnityEngine;

public class SkateboardSounds : MonoBehaviour
{
    public AK.Wwise.Event skateboardMovementSound;

    [SerializeField, Tooltip("The minimum amount of speed required for the sound to play")]
    private float _minimumSoundSpeed = 1;

    public float MinSoundSpeedSqr => _minimumSoundSpeed * _minimumSoundSpeed;

    private Rigidbody _rigidbody;
    private WheelCollider[] _wheelColliders = new WheelCollider[0];

    private void Start()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (_wheelColliders.Length == 0) _wheelColliders = GetComponentsInChildren<WheelCollider>();
    }

    private void Update()
    {
        // If grounded and above min speed
        if (IsAllGrounded() && _rigidbody.velocity.sqrMagnitude > MinSoundSpeedSqr)
        {
            // TODO: Uncomment when sound is added
            // skateboardMovementSound.Post(gameObject);
        }
    }

    /// <summary>
    /// Returns if all wheel colliders are on the ground
    /// </summary>
    /// <returns></returns>
    private bool IsAllGrounded()
    {
        foreach (var wc in _wheelColliders)
        {
            if (!wc.isGrounded) return false;
        }
        return true;
    }
}