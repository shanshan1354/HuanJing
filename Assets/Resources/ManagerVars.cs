using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    //音效
    public AudioClip buttonClip, diamondClip, fallClip, hitClip, jumpClip;
    //音效开启关闭图片
    public Sprite musicOn, musicOff;
    //角色预制体
    public GameObject characterPre;
    public GameObject deathEffect;
    //普通平台预制体
    public GameObject normalPlatformPre;
    //左边钉子
    public GameObject spikePlatformLeft;
    //右边钉子
    public GameObject spikePlatformRight;
    //钻石预制体
    public GameObject diamondPre;
    //皮肤预制体
    public GameObject skinChooseItemPre;
    //下一个平台位置生成的变化量
    public float nextXPos = 0.554f, nextYPos = 0.645f;
    //背景主题
    public List<Sprite> bgThemeSprite = new List<Sprite>();
    //平台主题
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    //商店中角色皮肤主题
    public List<Sprite> skinSpriteList = new List<Sprite>();
    //游戏中角色皮肤主题
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    //皮肤价格
    public List<int> skinPrice = new List<int>();
    //通用组合平台
    public List<GameObject> commonPlatformGroupList;
    //草地组合平台
    public List<GameObject> grassPlatformGroupList;
    //冬季组合平台
    public List<GameObject> winterPlatformGroupList;
    //定义皮肤名字
    public List<string> skinNameList = new List<string>();
    
    

}