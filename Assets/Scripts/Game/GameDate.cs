using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDate
{
    //是否再来一次
    public static bool isAgainGame = false;
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
    //钻石数量
    private int diamondCount;


    //设置是否第一次游戏
    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    //设置是否开启音乐
    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }
    //设置最好的成绩
    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    //设置选中的皮肤
    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    //设置未解锁的皮肤
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    //设置钻石数量
    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    //获取是否第一次游戏
    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    //获取是否开启音乐
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
    //获取最好的成绩
    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }
    //获取选中的皮肤
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    //获取未解锁的皮肤
    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }
    //获取钻石数量
    public int GetDiamondCount()
    {
        return diamondCount;
    }
}
