using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public int initSpawnCount = 5;
    //普通平台对象池
    private List<GameObject> normalPlatformList = new List<GameObject>();
    //通用平台对象池
    private List<GameObject> commonPlatformList = new List<GameObject>();
    //草地平台对象池
    private List<GameObject> grassPlatformList = new List<GameObject>();
    //冬季平台对象池
    private List<GameObject> winterPlatformList = new List<GameObject>();
    //左边钉子平台对象池
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    //右边钉子平台对象池
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    //死亡粒子特效对象池
    private List<GameObject> deathEffectList = new List<GameObject>();
    //钻石对象池
    private List<GameObject> diamondList = new List<GameObject>();

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    void Init()
    {
        //生成普通平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }
        //生成通用组合平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroupList.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroupList[j], ref commonPlatformList);
            }
        }
        //生成草地组合平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroupList.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroupList[j], ref grassPlatformList);
            }
        }
        //生成冬季组合平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroupList.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroupList[j], ref winterPlatformList);
            }
        }
        //生成左边钉子组合平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }
        //生成右边钉子组合平台对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }
        //生成死亡特效对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.deathEffect, ref deathEffectList);
        }
        //生成钻石对象池
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamondPre, ref diamondList);
        }
    }

    //生成对象，并添加到list集合中
    GameObject InstantiateObject(GameObject prefab, ref List<GameObject> addList)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }

    //get普通平台
    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            if (normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }
    //get通用组合平台
    public GameObject GetCommonPlatformGoup()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGroupList.Count);
        return InstantiateObject(vars.commonPlatformGroupList[ran], ref commonPlatformList);
    }
    //get草地组合平台
    public GameObject GetGrassPlatformGoup()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.grassPlatformGroupList.Count);
        return InstantiateObject(vars.grassPlatformGroupList[ran], ref grassPlatformList);
    }
    //get冬季组合平台
    public GameObject GetWinterPlatformGroup()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.winterPlatformGroupList.Count);
        return InstantiateObject(vars.winterPlatformGroupList[ran], ref winterPlatformList);
    }
    //get左边钉子组合平台
    public GameObject GetSpikePlatformLeftGroup()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }
    //get右边钉子组合平台
    public GameObject GetSpikePlatformRightGroup()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }
    //get粒子特效
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }
    //get钻石
    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObject(vars.diamondPre, ref diamondList);
    }
}