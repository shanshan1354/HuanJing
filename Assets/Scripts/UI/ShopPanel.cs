using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    private ManagerVars vars;
    private Button btn_Back;
    private Button btn_Select;
    private Button btn_Buy;
    private Transform parent;
    private Text txt_Name;
    private Text txt_Diamond;
    //选中的皮肤索引
    private int selectIndex;
    private void Awake()
    {
        btn_Back = transform.Find("Btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Select = transform.Find("Btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        btn_Buy = transform.Find("Btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);
        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("Txt_Name").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/Text").GetComponent<Text>();

        EventCenter.AddListener(EventDefine.ShowShopPanel, ShowShopPanel);

        vars = ManagerVars.GetManagerVars();
        
        //刚开始隐藏商店界面
        gameObject.SetActive(false);
    }
    private void Start()
    {
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, ShowShopPanel);
    }
    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(160 * (vars.skinSpriteList.Count + 2), 220);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go= Instantiate(vars.skinChooseItemPre, parent);
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3(160 * (i + 1), 0, 0);
            //如果皮肤未解锁，就设置成灰色，解锁了为白色
            if (GameManager.Instance.GetSkinUnlocked(i)==false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
        }
        //打开商店页面直接定位到选中的皮肤
        parent.GetComponent<RectTransform>().localPosition = new Vector3(-160 * GameManager.Instance.GetSelectedSkin(), 0, 0);
    }

    private void Update()
    {
        //获取选中的皮肤索引
        selectIndex = (int)Mathf.Round(parent.localPosition.x / -160f);
        //给选中的皮肤放大尺寸
        SetItemSize(selectIndex);
        //给选中的皮肤设置名字
        RefreshUI(selectIndex);
        if (Input.GetMouseButtonUp(0))
        {
            //设置皮肤的位置
            parent.transform.DOLocalMoveX(selectIndex * -160, 0.2f);
        }
    }
    //给皮肤设置大小，放大选中的皮肤
    private void SetItemSize(int selectIndex)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (i== selectIndex)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    //给皮肤添加名字,并设置皮肤是否解锁
    void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.skinNameList[selectIndex];
        txt_Diamond.text = GameManager.Instance.GetAllDiamonCount().ToString();
        if (GameManager.Instance.GetSkinUnlocked(selectIndex)==false)//未解锁
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
        }
        else//已解锁
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }
    //返回按钮点击
    void OnBackButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        //隐藏商店界面,显示主界面
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }
    //显示商店界面
    void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }
    //选中按钮点击
    void OnSelectButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        EventCenter.Broadcast(EventDefine.ChangeSkin, selectIndex);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        GameManager.Instance.SetSelectedSkin(selectIndex);
        gameObject.SetActive(false);
    }
    //购买皮肤按钮点击
    void OnBuyButtonClick()
    {
        //按钮点击音效
        EventCenter.Broadcast(EventDefine.PlayButtonAudioClip);

        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if (price>GameManager.Instance.GetAllDiamonCount())
        {
            EventCenter.Broadcast(EventDefine.ShowHint,"钻石不足了啦");
            return;
        }
        //购买皮肤
        GameManager.Instance.UpdateAllDiamondCount(-price);
        //解锁皮肤
        GameManager.Instance.SetSkinUnlockded(selectIndex);
        parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }
}
