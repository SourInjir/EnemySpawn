using UnityEngine;
using System.Collections.Generic;

public class GameMode : MonoBehaviour
{
    private const float SpawnDelay = 2.0f;

    [SerializeField] private List<SpawnPoint> _spawnerPoints;

    private WaitForSeconds _waitForSeconds;

    private void Awake(){
        _waitForSeconds = new WaitForSeconds(SpawnDelay);
    }

    private void Start()
    {
        StartCoroutine(SpawnProcess());
    }

    protected System.Collections.IEnumerator SpawnProcess()
    {
        while (true)
        {
            int index = Random.Range(0, _spawnerPoints.Count);
            SpawnPoint spawnPoint = _spawnerPoints[index];

            if (spawnPoint != null)
            {
                spawnPoint.Spawn();
            }

            yield return _waitForSeconds;
        }
    }
}