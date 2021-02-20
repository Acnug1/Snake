using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Food _foodTemplate;
    [SerializeField] [Range(0.1f, 30)] private float _spawnDelay;

    private Transform _spawner;
    private Vector3 _spawnPosition;

    private void Start()
    {
        _spawner = GetComponent<Transform>();
        StartCoroutine(SpawnFood(_spawnDelay));
    }

    private IEnumerator SpawnFood(float spawnDelay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnDelay);

        while (true)
        {
            _spawnPosition = _spawner.position + Random.insideUnitSphere * 19;

            Instantiate(_foodTemplate, _spawnPosition, Quaternion.identity, _spawner);

            yield return waitForSeconds;
        }
    }
}
