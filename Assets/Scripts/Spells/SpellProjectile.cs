
using System;
using Unity.XR.CoreUtils;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _hitPrefab;

    [SerializeField] private LayerMask _hitLayers;
    
    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hitLayers.Contains(other.gameObject.layer)) return;
        Instantiate(_hitPrefab).transform.position = transform.position;
        Destroy(gameObject);
    }
}
