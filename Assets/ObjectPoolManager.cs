using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _ObjectPoolEmptyHolder;

    private static GameObject _particleSystemEmpty;
    private static GameObject _gameObjectsEmpty;


    public enum PoolType
    {
        ParticleSystem,
        Gameobject,
        None
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        Instance = this;
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _ObjectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemEmpty = new GameObject("Particle Effects");
        _particleSystemEmpty.transform.SetParent(_ObjectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("GameObjects");
        _gameObjectsEmpty.transform.SetParent(_ObjectPoolEmptyHolder.transform);

    }
    /// <summary>
    /// 预热对象池，提前生成多个静态对象，等到需要用的时候Active就行了
    /// </summary>
    /// <param name="objectToPreheat">需要预热的游戏对象</param>
    /// <param name="count">预热数量</param>
    /// <param name="poolType">类型：None、对象或者粒子效果</param>
    public static void PreheatPool(GameObject objectToPreheat, int count, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToPreheat.name);
        //如果对象池不存在，则创建一个
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToPreheat.name };
            ObjectPools.Add(pool);
        }
        for (int i = 0; i < count; i++)
        {
            GameObject newObject = Instantiate(objectToPreheat, Vector3.zero, Quaternion.identity);
            newObject.SetActive(false);

            //放入对象池
            pool.InactiveObjects.Add(newObject);

            //设置父物体
            GameObject parentObject = SetParentObject(poolType);
            if (parentObject != null)
            {
                newObject.transform.SetParent(parentObject.transform);
            }
        }
    }

    /// <summary>
    /// 将对象创建进对象池，使用方法类似于Instantiate
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        //如果对象池不存在，则创建一个
        if(pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //检查池中是否有非活动对象
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            //找到空对象的父类
            GameObject parentObject = SetParentObject(poolType);

            //如果没有活动对象，则创建一个
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if(parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
                //TODO：这个代码在此处可能有些bug，于是有了这一条：
                //spawnableObj.transform.SetParent(parentTransform);
                //spawnableObj.transform.position = parentTransform.position;
                //spawnableObj.transform.rotation = parentTransform.rotation;
                //解释如下：如果赋的是transform，回收再拿出来父物体是无法变更的，所以两个物体都需要生成同名的子物体就会有冲突，所以需要拿到transform的时候重新赋一次父物体保证正确
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);

            if (spawnableObj.GetComponent<TrailRenderer>())
            {
                TrailRenderer trailRenderer = spawnableObj.GetComponent<TrailRenderer>();
                trailRenderer.Clear();
            }

            spawnableObj.SetActive(true);
        }

        return spawnableObj;
        
    }

    //重载：生成在父类上
    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        //如果对象池不存在，则创建一个
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //检查池中是否有非活动对象
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            //如果没有活动对象，则创建一个
            spawnableObj = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;

    }

    /// <summary>
    /// 类似于Destory，但是是收回对象池
    /// </summary>
    /// <param name="obj">需要回收的对象</param>
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);//删除(clone)

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("尝试去寻找一个未被池化的对象：" + obj.name);
        }

        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }

    }

    //重载：时间t后回收
    public static void ReturnObjectToPool(GameObject obj, float t)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); // 删除(clone)
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("尝试去寻找一个未被池化的对象：" + obj.name);
        }
        else
        {
            if (obj.activeSelf)
            {
                // 延迟执行ReturnObject方法
                IEnumerator DelayedReturnObject(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    if (obj && obj.activeSelf)
                    {
                        obj.SetActive(false);
                        pool.InactiveObjects.Add(obj);
                    }
                }
                // 启动延迟执行的协程
                Instance.StartCoroutine(DelayedReturnObject(t));
            }
        }

    }

    /// <summary>
    /// 移除对象池中的对象，如果你确定之后也不再使用的话……
    /// </summary>
    /// <param name="obj">你想要清理的冗余对象</param>
    /// <param name="ConfirmDestory">默认为不确定，只会删除未在活动的对象</param>
    public static void DestoryObjectInPool(GameObject obj, bool ConfirmDestory = false)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if(pool == null)
        {
            Debug.LogWarning(obj.name +  "未在池中");
        }
        else
        {
            if (obj.activeSelf && ConfirmDestory == false)
            {
                ReturnObjectToPool(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// 清理所有对象，如果ConfirmDestory为false，则将所有池内对象Inactive
    /// </summary>
    /// <param name="ConfirmDestory"></param>
    public static void DestoryObjectInPool(bool ConfirmDestory = false)
    {
        foreach (PooledObjectInfo pool in ObjectPools)
        {
            if (ConfirmDestory)
            {
                foreach (GameObject obj in pool.InactiveObjects)
                {
                    Destroy(obj);
                }
                pool.InactiveObjects.Clear();
            }
            else
            {
                foreach (GameObject obj in pool.InactiveObjects)
                {
                    ReturnObjectToPool(obj);
                }
            }
        }
    }


    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemEmpty;

            case PoolType.Gameobject:
                return _gameObjectsEmpty;

            case PoolType.None:
                return null;

            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
