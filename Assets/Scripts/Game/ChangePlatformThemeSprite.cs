using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformThemeSprite : MonoBehaviour
{
    public SpriteRenderer[] platformSpriteRenderer;
    public GameObject obstacle;
    //平台掉落时间计时器
    private bool startTimer;
    //平台掉落时间
    private float fallTime;
    private Rigidbody2D my_Body;

    private void Awake()
    {
        my_Body = GetComponent<Rigidbody2D>();
    }
    public void Init(Sprite sprite,float fallTime,int obstacleDir)
    {
        my_Body.bodyType = RigidbodyType2D.Static;//给刚生成的平台设置成静态的
        startTimer = true;//生成平台时候开启计时器
        this.fallTime = fallTime;//获取平台掉落时间
        for (int i = 0; i < platformSpriteRenderer.Length; i++)
        {
            platformSpriteRenderer[i].sprite = sprite;
        }

        if (obstacleDir==0)//障碍物朝右边
        {
            if (obstacle!=null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x,
                obstacle.transform.localPosition.y, 0);
            }
        }
    }

    private void Update()
    {
        //游戏还没开始，或者开始了，角色还没进行移动
        if (GameManager.Instance.IsGameStart==false||GameManager.Instance.IsPlayerMove==false)
        {
            return;
        }
        //如果计时器开启
        if (startTimer)
        {
            fallTime -= Time.deltaTime;
            if (fallTime<0)
            {
                //平台掉落
                startTimer = false;
                if (my_Body.bodyType!=RigidbodyType2D.Dynamic)
                {
                    my_Body.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DelayHide());
                }
            }
        }
        if (transform.position.y-Camera.main.transform.position.y<-6)
        {
            StartCoroutine(DelayHide());
        }
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}