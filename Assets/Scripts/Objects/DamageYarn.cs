using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageYarn : MonoBehaviour
{
    [SerializeField] private bool _useCollision = true;
    [SerializeField] private bool _useTrigger = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.Damage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.Damage();
        }
    }
}
