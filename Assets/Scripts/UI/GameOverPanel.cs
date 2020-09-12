using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text txt_Score, txt_BestScore, txt_AddDiamondCount;
    public Button btn_Restart, btn_Rank, btn_Home;
    public Image img_New;
    private void Awake()
    {
        gameObject.SetActive(false);
        btn_Restart.onClick.AddListener(OnResrartButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);
    }

    //显示游戏结束面板
    void ShowGameOverPanel()
    {
        if (GameManager.Instance.GetScore()>GameManager.Instance.GetBestScore())
        {
            txt_BestScore.text = "最高分  " + GameManager.Instance.GetScore();
            img_New.gameObject.SetActive(true);
        }
        else
        {
            txt_BestScore.text = "最高分  " + GameManager.Instance.GetBestScore();
            img_New.gameObject.SetActive(false);
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetScore());
        txt_Score.text = GameManager.Instance.GetScore().ToString();
        txt_AddDiamondCount.text = "+" + GameManager.Instance.GetDiamondCount();
        //更新总的钻石数量
        GameManager.Instance.UpdateAllDiamondCount(GameManager.Instance.GetDiamondCount());

        gameObject.SetActive(true);
    }

    //在来一次按钮点击
    void OnResrartButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GameDate.isAgainGame = true;
    }
    //排行榜按钮点击
    void OnRankButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    //返回主界面按钮点击
    void OnHomeButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameDate.isAgainGame = false;
    }
}
