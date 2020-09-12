﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    private Image img_Bg;
    private Text txt_Hint;
    private void Awake()
    {
        img_Bg = GetComponent<Image>();
        txt_Hint = GetComponentInChildren<Text>();
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        txt_Hint.color = new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b, 0);

        EventCenter.AddListener<string>(EventDefine.ShowHint, ShowHint);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowHint, ShowHint);
    }
    //显示购买皮肤钻石不足信息
    private void ShowHint(string text)
    {
        StopCoroutine("Delay");
        transform.localPosition = new Vector3(0, -70, 0);
        txt_Hint.text = text;
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() =>
        {
            StartCoroutine("Delay");
        });
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.5f), 0.1f);
        txt_Hint.DOColor(new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b, 1), 0.1f);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveY(70, 0.3f);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.1f);
        txt_Hint.DOColor(new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b,0), 0.1f);
    }
}
