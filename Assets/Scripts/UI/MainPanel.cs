using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    //开始按钮
    private Button btn_Start;
    //商店按钮
    private Button btn_Shop;
    //重置按钮
    private Button btn_Rest;
    //排行榜按钮
    private Button btn_Rank;
    //音效按钮
    private Button btn_Sound;

    private ManagerVars vars;
    void Awake()
    {
        
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);

        Init();
    }
    private void Start()
    {
        SetSound();
        if (GameDate.isAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanle);
            gameObject.SetActive(false);
        }
        ChangeSkin(GameManager.Instance.GetSelectedSkin());
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    //初始化
    void Init()
    {
        btn_Start = transform.Find("Btn_Start").GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStarButtonClick);

        btn_Shop = transform.Find("Btns/Btn_Shop").GetComponent<Button>();
        btn_Shop.onClick.AddListener(OnShopButtonClick);

        btn_Rest = transform.Find("Btns/Btn_Reset").GetComponent<Button>();
        btn_Rest.onClick.AddListener(OnResetButtonClick);

        btn_Rank = transform.Find("Btns/Btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankButtonClick);

        btn_Sound = transform.Find("Btns/Btn_Sound").GetComponent<Button>();
        btn_Sound.onClick.AddListener(OnSoundButtonClick);

    }
    //开始按钮点击后调用
    void OnStarButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);
        //广播ShowGamePanle方法
        EventCenter.Broadcast(EventDefine.ShowGamePanle);
        gameObject.SetActive(false);
        //游戏开始
        GameManager.Instance.IsGameStart = true;
    }

    //商店按钮点击后调用
    void OnShopButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        //隐藏主界面，显示商店界面
        gameObject.SetActive(false);
    }
    //重置按钮点击
    void OnResetButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }
    //排行榜按钮点击后调用
    void OnRankButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    //音效按钮点击后调用
    void OnSoundButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        SetSound();
    }
    //设置音效图片
    void SetSound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn,GameManager.Instance.GetIsMusicOn());
    }
    //显示主界面
    void ShowMainPanel()
    {
        gameObject.SetActive(true);
    }
    //更换皮肤UI
    void ChangeSkin(int skinIndex)
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[skinIndex];
    }
}
