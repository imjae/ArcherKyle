using UnityEngine;
using System.Collections;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager singleton;
    public static ObjectPoolManager GetInstance() { return singleton; }

    public ObjectPool<PoolableObject> arrowPool = new ObjectPool<PoolableObject>();

    public Arrow arrowPrefab;

    void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
    }

    void Start()
    {
        // arrowPool = new ObjectPool<PoolableObject>(5, () =>
        // {
        //     Arrow arrow = Instantiate(arrowPrefab);
        //     arrow.Create(arrowPool);
        //     return arrow;
        // });

        arrowPool.Allocate();
    }

    void OnDestroy()
    {
        arrowPool.Dispose();
        singleton = null;
    }
}