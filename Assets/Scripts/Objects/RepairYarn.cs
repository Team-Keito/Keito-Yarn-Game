using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairYarn : MonoBehaviour
{
    [SerializeField] private bool _useCollision = true;
    [SerializeField] private bool _useTrigger = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_useCollision && collision.gameObject.TryGetComponent<IRepairable>(out IRepairable target))
        {
            target.Repair();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_useTrigger && other.TryGetComponent<IRepairable>(out IRepairable target))
        {
            target.Repair();
        }
    }
}
