using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_Score;
    private Text txt_Diamond;

    void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePanle, ShowGamePanle);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamonText, UpdateDiamondText);
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanle, ShowGamePanle);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamonText, UpdateDiamondText);
    }
    void Init()
    {
        gameObject.SetActive(false);
        btn_Pause = transform.Find("Btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnButtonPuaseClick);

        btn_Play = transform.Find("Btn_Play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnButtonPlayClick);

        txt_Score = transform.Find("Txt_Score").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/Txt_Diamond").GetComponent<Text>();
    }

    //显示自身GamePanel面板
    void ShowGamePanle()
    {
        gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
    }
    //暂停按钮点击后调用
    void OnButtonPuaseClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        //暂停游戏
        GameManager.Instance.IsGamePause = true;
        Time.timeScale = 0;
    }

    //开始播放按钮点击后调用
    void OnButtonPlayClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        //继续游戏
        GameManager.Instance.IsGamePause = false;
        Time.timeScale = 1;
    }
    //更新成绩显示
    void UpdateScoreText(int score)
    {
        txt_Score.text = score.ToString();
    }
    //更新钻石数量显示
    void UpdateDiamondText(int diamond)
    {
        txt_Diamond.text = diamond.ToString();
    }
}
