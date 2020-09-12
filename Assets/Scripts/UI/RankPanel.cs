using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankPanel : MonoBehaviour
{
    public Text[] scores;

    private Button btn_Back;
    private GameObject scoresGO;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankPanel, ShowRankPanel);

        btn_Back = transform.Find("Btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Back.GetComponent<Image>().color = new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g, btn_Back.GetComponent<Image>().color.b, 0);
        scoresGO = transform.Find("ScoreList").gameObject;
        scoresGO.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, ShowRankPanel);
    }
    //显示排行榜界面
    void ShowRankPanel()
    {
        gameObject.SetActive(true);
        int[] scoreArr = GameManager.Instance.GetScoreArr();
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].text = scoreArr[i].ToString();
        }
        btn_Back.GetComponent<Image>().DOColor(new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g, btn_Back.GetComponent<Image>().color.b, 0.3f), 0.3f);
        scoresGO.transform.DOScale(Vector3.one, 0.3f);
    }
    //返回按钮点击
    void OnBackButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        btn_Back.GetComponent<Image>().DOColor(new Color(btn_Back.GetComponent<Image>().color.r,
            btn_Back.GetComponent<Image>().color.g, btn_Back.GetComponent<Image>().color.b, 0), 0.3f);
        scoresGO.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
