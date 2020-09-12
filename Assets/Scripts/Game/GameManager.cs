using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    //游戏是否开始
    public bool IsGameStart { get; set; }
    //游戏是否结束
    public bool IsGameOver { get; set; }
    //游戏是否暂停
    public bool IsGamePause { get; set; }
    //角色是否移动
    public bool IsPlayerMove { get; set; }
    //分数
    private int score;
    //获取本局钻石数
    private int diamond;

    private ManagerVars vars;
    private GameDate date;
    //是否第一次游戏
    private bool isFirstGame;
    //是否开启音乐
    private bool isMusicOn;
    //最好的成绩
    private int[] bestScoreArr;
    //选中的皮肤
    private int selectSkin;
    //未解锁的皮肤
    private bool[] skinUnlocked;
    //总钻石数量
    private int diamondCount;

    void Awake()
    {

        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond);
        Instance = this;
        InitGameDate();
    }
    private void Start()
    {
        if (GameDate.isAgainGame)
        {
            IsGameStart = true;
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddDiamond);
    }
    //保存游戏数据
    void Save()
    {
        date = new GameDate();
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameDate.date"))
            {
                date.SetIsFirstGame(isFirstGame);
                date.SetIsMusicOn(isMusicOn);
                date.SetBestScoreArr(bestScoreArr);
                date.SetSelectSkin(selectSkin);
                date.SetSkinUnlocked(skinUnlocked);
                date.SetDiamondCount(diamondCount);
                bf.Serialize(fs, date);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    //获取游戏数据
    void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameDate.date", FileMode.Open))
            {
                date = (GameDate)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    //初始化游戏数据
    void InitGameDate()
    {
        Read();
        if (date != null)
        {
            isFirstGame = false;
        }
        else
        {
            isFirstGame = true;

        }
        //如果是第一次开始游戏
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            Save();
        }
        else
        {
            isMusicOn = date.GetIsMusicOn();
            bestScoreArr = date.GetBestScoreArr();
            selectSkin = date.GetSelectSkin();
            skinUnlocked = date.GetSkinUnlocked();
            diamondCount = date.GetDiamondCount();

        }
    }
    //判断角色是否移动
    void PlayerMove()
    {
        IsPlayerMove = true;
    }
    //增加分数
    void AddScore()
    {
        score++;
        EventCenter.Broadcast<int>(EventDefine.UpdateScoreText, score);
    }
    //增加钻石数量
    void AddDiamond()
    {
        diamond++;
        EventCenter.Broadcast<int>(EventDefine.UpdateDiamonText, diamond);
    }
    //获得分数
    public int GetScore()
    {
        return score;
    }
    //保存成绩
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        //降序
        list.Sort((x, y) => -x.CompareTo(y));
        bestScoreArr = list.ToArray();
        if (score>=bestScoreArr[2])
        {
            bestScoreArr[2] = score;
        }
        list = bestScoreArr.ToList();
        list.Sort((x, y) => -x.CompareTo(y));
        bestScoreArr = list.ToArray();
    }
    //获取最高分
    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }
    //获取成绩数组
    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        //降序
        list.Sort((x, y) => -x.CompareTo(y));
        bestScoreArr = list.ToArray();
        return bestScoreArr;
    }
    //获得本局游戏吃到的钻石数量
    public int GetDiamondCount()
    {
        return diamond;
    }
    //更新总的钻石数量
    public void UpdateAllDiamondCount(int value)
    {
        diamondCount += value;
        Save();
    }
    //获得所有的钻石数量
    public int GetAllDiamonCount()
    {
        return diamondCount;
    }
    //获取是否解锁的皮肤
    public bool GetSkinUnlocked(int skinIndex)
    {
        return skinUnlocked[skinIndex];
    }
    //设置解锁的皮肤
    public void SetSkinUnlockded(int skinIndex)
    {
        skinUnlocked[skinIndex] = true;
        Save();
    }
    //选中皮肤
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    //获取选中的皮肤的索引
    public int GetSelectedSkin()
    {
        return selectSkin;
    }
    //设置游戏音效
    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
        Save();
    }

    //获取是否开启音效
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
}
