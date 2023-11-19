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
    /// Ԥ�ȶ���أ���ǰ���ɶ����̬���󣬵ȵ���Ҫ�õ�ʱ��Active������
    /// </summary>
    /// <param name="objectToPreheat">��ҪԤ�ȵ���Ϸ����</param>
    /// <param name="count">Ԥ������</param>
    /// <param name="poolType">���ͣ�None�������������Ч��</param>
    public static void PreheatPool(GameObject objectToPreheat, int count, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToPreheat.name);
        //�������ز����ڣ��򴴽�һ��
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToPreheat.name };
            ObjectPools.Add(pool);
        }
        for (int i = 0; i < count; i++)
        {
            GameObject newObject = Instantiate(objectToPreheat, Vector3.zero, Quaternion.identity);
            newObject.SetActive(false);

            //��������
            pool.InactiveObjects.Add(newObject);

            //���ø�����
            GameObject parentObject = SetParentObject(poolType);
            if (parentObject != null)
            {
                newObject.transform.SetParent(parentObject.transform);
            }
        }
    }

    /// <summary>
    /// �����󴴽�������أ�ʹ�÷���������Instantiate
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="poolType"></param>
    /// <returns></returns>
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        //�������ز����ڣ��򴴽�һ��
        if(pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //�������Ƿ��зǻ����
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            //�ҵ��ն���ĸ���
            GameObject parentObject = SetParentObject(poolType);

            //���û�л�����򴴽�һ��
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if(parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
                //TODO����������ڴ˴�������Щbug������������һ����
                //spawnableObj.transform.SetParent(parentTransform);
                //spawnableObj.transform.position = parentTransform.position;
                //spawnableObj.transform.rotation = parentTransform.rotation;
                //�������£����������transform���������ó������������޷�����ģ������������嶼��Ҫ����ͬ����������ͻ��г�ͻ��������Ҫ�õ�transform��ʱ�����¸�һ�θ����屣֤��ȷ
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

    //���أ������ڸ�����
    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        //�������ز����ڣ��򴴽�һ��
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //�������Ƿ��зǻ����
        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            //���û�л�����򴴽�һ��
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
    /// ������Destory���������ջض����
    /// </summary>
    /// <param name="obj">��Ҫ���յĶ���</param>
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);//ɾ��(clone)

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("����ȥѰ��һ��δ���ػ��Ķ���" + obj.name);
        }

        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }

    }

    //���أ�ʱ��t�����
    public static void ReturnObjectToPool(GameObject obj, float t)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); // ɾ��(clone)
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("����ȥѰ��һ��δ���ػ��Ķ���" + obj.name);
        }
        else
        {
            if (obj.activeSelf)
            {
                // �ӳ�ִ��ReturnObject����
                IEnumerator DelayedReturnObject(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    if (obj && obj.activeSelf)
                    {
                        obj.SetActive(false);
                        pool.InactiveObjects.Add(obj);
                    }
                }
                // �����ӳ�ִ�е�Э��
                Instance.StartCoroutine(DelayedReturnObject(t));
            }
        }

    }

    /// <summary>
    /// �Ƴ�������еĶ��������ȷ��֮��Ҳ����ʹ�õĻ�����
    /// </summary>
    /// <param name="obj">����Ҫ������������</param>
    /// <param name="ConfirmDestory">Ĭ��Ϊ��ȷ����ֻ��ɾ��δ�ڻ�Ķ���</param>
    public static void DestoryObjectInPool(GameObject obj, bool ConfirmDestory = false)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if(pool == null)
        {
            Debug.LogWarning(obj.name +  "δ�ڳ���");
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
    /// �������ж������ConfirmDestoryΪfalse�������г��ڶ���Inactive
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
