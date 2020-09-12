using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private ManagerVars vars;
    private void Awake()
    {
        //生成随机背景
        vars = ManagerVars.GetManagerVars();
        int varsIndex = Random.Range(0, vars.bgThemeSprite.Count);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = vars.bgThemeSprite[varsIndex];
    }
}