using System.Collections;
using UnityEngine;

public class Earthshard : MonoBehaviour
{
    [SerializeField] private GameObject _castPrefab;
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private GameObject _playerRightHand;

    [SerializeField] private SpellHandler _spellHandler;
    
    private void Start()
    {
        _spellHandler.Spells.Add("earth shard", CastEarthshard); }

    private void CastEarthshard(string modifier)
    {
        Vector3 spawnOffset = _playerRightHand.transform.forward * 0.25f;

        GameObject castEffect = Instantiate(_castPrefab);
        castEffect.transform.position = _playerRightHand.transform.position + spawnOffset;
        castEffect.transform.rotation = _playerRightHand.transform.rotation;
        StartCoroutine(DestoryAfterDelay(castEffect, 1));
        
        GameObject projectile = Instantiate(_projectilePrefab);
        projectile.transform.position = _playerRightHand.transform.position + spawnOffset;
        projectile.transform.rotation = _playerRightHand.transform.rotation;
        StartCoroutine(DestoryAfterDelay(projectile, 7));
    }
    
    private IEnumerator DestoryAfterDelay(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }
}