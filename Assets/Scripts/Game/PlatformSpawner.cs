using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//组合平台类型
enum PlatformGroupType
{
    Winter,
    Grass
}
public class PlatformSpawner : MonoBehaviour
{
    //平台掉落时间
    public float fallTime;
    //平台掉落时间系数
    public float multiple;
    //平台最小掉落时间
    public float minFallTime;
    //里程碑数
    public int milestoneCount=10;
    //平台初始位置
    public Vector3 startSpawnPos;
    //是否朝左边生成
    private bool isLeftSpawn = false;
    //钉子组合平台的生成是否在左边
    private bool isSpikeSpawnLeft = false;
    //是否生成了钉子，若是，就往钉子方向生成平台
    private bool isSpawnSpike = false;
    //生成平台数量
    private int spawnPlatformCount;
    //生成钉子平台之后需要在钉子方向生成的平台数量
    private int afterSpawnSpikeSpawnCount;
    private ManagerVars vars;
    //组合平台类型
    private PlatformGroupType groupType;
    //选中的平台主题
    private Sprite selectPlatformSprite;
    //平台生成位置
    private Vector3 spawnPlatformPos;
    //钉子方向平台的位置
    private Vector3 spikeDirPlatformPos;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        //添加监听DecidePath方法
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    void Start()
    {
        //随机平台主题
        RandomPlatformTheme();
        //平台初始位置
        spawnPlatformPos = startSpawnPos;
        //刚开始生成平台路径次数
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }
        //生成角色
        GameObject go = Instantiate(vars.characterPre);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStart&&GameManager.Instance.IsGameOver==false)
        {
            UpdateFallTime();
        }
    }
    //个更新平台掉落时间
    void UpdateFallTime()
    {
        if (GameManager.Instance.GetScore()>milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multiple;
            if (fallTime<minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    //确定生成路径
    void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }
        if (spawnPlatformCount <= 0)
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
        }
        else
        {
            spawnPlatformCount--;
        }
        SpawnPlatform();
    }
    //生成平台
    void SpawnPlatform()
    {
        //随机组合平台障碍物生成位置
        int ranObstacleDir = Random.Range(0, 2);

        if (spawnPlatformCount > 0)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        else if (spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            if (ran == 0)
            {
                //生成通用组合平台
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            else if (ran == 1)
            {
                //生成主题组合平台
                switch (groupType)
                {
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //生成钉子组合平台
                int value = -1;
                if (isLeftSpawn)//若往左边生成平台
                {
                    value = 1;//往右边生成钉子
                }
                else
                {
                    value = 0;//表示往左边生成钉子
                }
                SpawnSpikePlatformGroup(value,ranObstacleDir);//生成钉子组合平台

                isSpawnSpike = true;//生成了钉子
                afterSpawnSpikeSpawnCount = 4;
                if (isSpikeSpawnLeft)//钉子生成在左边
                {
                    spikeDirPlatformPos = new Vector3(spawnPlatformPos.x - 1.65f, 
                        spawnPlatformPos.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(spawnPlatformPos.x + 1.65f,
                        spawnPlatformPos.y + vars.nextYPos, 0);
                }
            }
        }

        //随机生成钻石
        int ranSpawnDiamond = Random.Range(0, 10);
        if (ranSpawnDiamond == 6&&GameManager.Instance.IsPlayerMove)
        {
            GameObject go= ObjectPool.Instance.GetDiamond();
            go.transform.position = new Vector3(spawnPlatformPos.x, spawnPlatformPos.y + 0.5f, 0);
            go.SetActive(true);
        }
        //定义平台朝左边（右边）生成
        if (isLeftSpawn)
        {
            spawnPlatformPos = new Vector3(spawnPlatformPos.x - vars.nextXPos,
                spawnPlatformPos.y + vars.nextYPos, 0);
        }
        else
        {
            spawnPlatformPos = new Vector3(spawnPlatformPos.x + vars.nextXPos, 
                spawnPlatformPos.y + vars.nextYPos, 0);
        }
    }
    //生成普通平台
    void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = spawnPlatformPos;
        //更换平台主题
        go.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    
    //生成通用组合平台
    void SpawnCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatformGoup();
        go.transform.position = spawnPlatformPos;
        go.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    //生成草地组合平台
    void SpawnGrassPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatformGoup();
        go.transform.position = spawnPlatformPos;
        go.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    //生成冬季组合平台
    void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        go.transform.position = spawnPlatformPos;
        go.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    //生成钉子组合平台
    void SpawnSpikePlatformGroup(int value,int ranObstacleDir)
    {
        GameObject go;
        if (value ==0)//0表示钉子生成在左边
        {
            isSpikeSpawnLeft = true;
            go = ObjectPool.Instance.GetSpikePlatformLeftGroup();
        }
        else//1表示钉子生成在右边
        {
            isSpikeSpawnLeft = false;
            go = ObjectPool.Instance.GetSpikePlatformRightGroup();
        }
        go.transform.position = spawnPlatformPos;
        go.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    //往钉子方向生成平台,也包括原路径的平台
    void AfterSpawnSpike()
    {
        if (afterSpawnSpikeSpawnCount>0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                if (i==0)//生成原先路径平台
                {
                    temp.transform.position = spawnPlatformPos;
                    if (isSpikeSpawnLeft)//若钉子方向是左边，则平台生成在右边
                    {
                        spawnPlatformPos = new Vector3(spawnPlatformPos.x + vars.nextXPos,
                spawnPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spawnPlatformPos = new Vector3(spawnPlatformPos.x - vars.nextXPos,
                spawnPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                else//生成钉子方向平台
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (isSpikeSpawnLeft)//生成左边钉子方向的平台
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos,
                spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else//生成右边钉子方向的平台
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos,
                spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<ChangePlatformThemeSprite>().Init(selectPlatformSprite, fallTime, 0);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
    //随机平台主题
    void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];
        if (ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
}
