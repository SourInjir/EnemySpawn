using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private const float DirectionAngle = 360f;

    [SerializeField] private Enemy _prefab;

    private int _defaultCapacity = 20;
    private int _maxSize = 100;
    

    public ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolItem, true, _defaultCapacity, _maxSize);

        for (int i = 0; i < _defaultCapacity; i++)
        {
            _pool.Release(CreatePooledItem());
        }

    }

    private Enemy CreatePooledItem()
    {
        Enemy obj = Instantiate(_prefab);
        return obj;
    }

    private void OnTakeFromPool(Enemy obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(Enemy obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyPoolItem(Enemy obj)
    {
        Destroy(obj.gameObject);
    }

    public Enemy GetObject()
    {
        return _pool.Get();
    }

    public void ReturnWithDelay(Enemy obj, float delay)
    {
        StartCoroutine(ReturnCoroutine(obj, delay));
    }

    protected System.Collections.IEnumerator ReturnCoroutine(Enemy obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Release(obj);
    }

    public Enemy SpawnObject(Vector3 position)
    {
        Enemy obj = GetObject();
        obj.transform.position = position;
        obj.SetTargetDirection(GetRandomDirection());
        return obj;
    }

    private Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, DirectionAngle);

        Vector3 direction = new Vector3(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        );

        return direction.normalized;
    }
}
