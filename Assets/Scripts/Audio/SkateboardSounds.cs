using UnityEngine;
using AK.Wwise;

public class SkateboardSounds : MonoBehaviour
{
    private const int MAX_VOLUME = 100;
    public AK.Wwise.Event skateboardMovementSound;

    [SerializeField, Tooltip("The minimum amount of speed required for the sound to play")]
    private float _minimumSoundSpeed = 0.5f;

    [SerializeField, Tooltip("The speed at which max volume will play (100% volume). All faster speed is fixed to 100% volume")]
    private float _maximumSpeedVolume = 3;
    [SerializeField, Range(0.001f, 10)]
    private float _hitForceMultiplier = 1;

    public float MinSoundSpeedSqr => _minimumSoundSpeed * _minimumSoundSpeed;

    private Rigidbody _rigidbody;
    private WheelCollider[] _wheelColliders = new WheelCollider[0];

    private void Start()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (_wheelColliders.Length == 0) _wheelColliders = GetComponentsInChildren<WheelCollider>();
        AkSoundEngine.SetRTPCValue("SkateSpeed", 0f);
        skateboardMovementSound.Post(gameObject);
    }

    private void Update()
    {
        // If grounded and above min speed
        if (IsAllGrounded() && _rigidbody.velocity.sqrMagnitude > MinSoundSpeedSqr)
        {
            var volume = SpeedToVolume();
            // TODO: Uncomment when sound is added
            // skateboardMovementSound.Post(gameObject);

            // Map skateboard speed to RTPC value (0-100)
            float speedRTPCValue = Mathf.Clamp(volume, 0f, 100f);

            // Set the RTPC value in Wwise
            AkSoundEngine.SetRTPCValue("SkateSpeed", speedRTPCValue);

            // Post the sound event to play the sound with the updated RTPC value
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Yarn"))
        {
            // Apply force to get skateboard to move
            for (int i = 0; i < other.contactCount; i++)
            {
                var contact = other.GetContact(i);
                _rigidbody.AddForceAtPosition(contact.normal * _hitForceMultiplier, contact.point);
            }
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

    /// <summary>
    /// Convert speed value (M-N; M=min speed, N=max speed) to volume value (0-100)
    /// </summary>
    /// <returns>Volume value between 0 and 100</returns>
    public float SpeedToVolume() => Mathf.Clamp(_rigidbody.velocity.magnitude - _minimumSoundSpeed, 0, _maximumSpeedVolume) / _maximumSpeedVolume * MAX_VOLUME;
}