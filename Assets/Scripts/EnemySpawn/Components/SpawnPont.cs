using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    public void Spawn()
    {
        if (_spawner != null)
        {
            _spawner.SpawnObject(transform.position);
        }
    }
}