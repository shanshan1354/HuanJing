using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using UnityEngine.SceneManagement;

public class ResetPanel : MonoBehaviour
{
    private Button btn_No;
    private Button btn_Yes;
    private Image img_Bg;
    private Image img_Dialog;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPanel, ShowResetPanel);

        btn_No = transform.Find("Dialog/Btn_No").GetComponent<Button>();
        btn_No.onClick.AddListener(OnNoButtonClick);
        btn_Yes= transform.Find("Dialog/Btn_Yes").GetComponent<Button>();
        btn_Yes.onClick.AddListener(OnYesButtonClick);

        img_Bg = transform.Find("Bg").GetComponent<Image>();
        img_Dialog = transform.Find("Dialog").GetComponent<Image>();
        //背景透明度设置为0
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        //提示框大小设置为0
        img_Dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, ShowResetPanel);
    }
    //显示重置界面
    private void ShowResetPanel()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        img_Dialog.transform.DOScale(Vector3.one, 0.3f);
    }
    //No按钮点击
    private void OnNoButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        img_Dialog.transform.DOScale(Vector3.zero, 0.3f);
        gameObject.SetActive(false);
    }
    //Yes按钮点击
    private void OnYesButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        //删除保存的数据
        if (File.Exists(Application.persistentDataPath + "/GameDate.date"))
        {
            File.Delete(Application.persistentDataPath + "/GameDate.date");
        }

        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        img_Dialog.transform.DOScale(Vector3.zero, 0.3f);
        gameObject.SetActive(false);
        //重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
